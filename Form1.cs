using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Threading;
using Microsoft.VisualBasic.ApplicationServices;

namespace Lab3
{
    public partial class Form1 : Form
    {
        DataSet ds = new DataSet();
        BindingSource bsParent;
        BindingSource bsChild;
        SqlDataAdapter parentAdapter;
        SqlDataAdapter childAdapter;
        string connectionString;
        Dictionary<string, Dictionary<string, string>> tableQueries = new Dictionary<string, Dictionary<string, string>>();

        private string selectedTable;
        private int? currentEditedRow = null;


        public Form1()
        {
            InitializeComponent();
            this.dataGridViewParent.AllowUserToAddRows = true;
            this.dataGridViewParent.EditMode = DataGridViewEditMode.EditOnEnter;
            LoadConfiguration();
            LoadTableNamesToListBox();
        }



        private void LoadConfiguration()
        {
            XDocument config = XDocument.Load("C:\\Users\\belde\\OneDrive\\Desktop\\SGBD\\Lab1\\Lab1\\FisierConfigurare.xml");
            connectionString = config.Element("Configuration").Element("Database").Element("ConnectionString").Value;
            LoadQueries(config);

            foreach (var table in config.Element("Configuration").Element("Tables").Elements("Table"))
            {
                string tableName = table.Attribute("name").Value;
                string selectCommand = tableQueries[tableName]["select"];

                SqlDataAdapter adapter = new SqlDataAdapter(selectCommand, connectionString);
                adapter.Fill(ds, tableName);
            }

            LoadRelationships(config);
        }

        private void LoadRelationships(XDocument config)
        {
            var relationships = config.Element("Configuration").Element("Relationships").Elements("Relationship");
            foreach (var relationship in relationships)
            {
                string parentTable = relationship.Attribute("parentTable").Value;
                string childTable = relationship.Attribute("childTable").Value;
                string parentColumn = relationship.Attribute("parentPrimaryKey").Value;
                string childColumn = relationship.Attribute("childForeignKey").Value;

                DataColumn parentColumnObj = ds.Tables[parentTable].Columns[parentColumn];
                DataColumn childColumnObj = ds.Tables[childTable].Columns[childColumn];

                DataRelation relation = new DataRelation($"{parentTable}_{childTable}", parentColumnObj, childColumnObj);
                ds.Relations.Add(relation);
            }
        }


        private void LoadQueries(XDocument config)
        {
            var queries = config.Element("Configuration").Element("Queries").Elements("Query");
            foreach (var query in queries)
            {
                string table = query.Attribute("table").Value;
                string operation = query.Attribute("operation").Value;
                string command = query.Attribute("command").Value;

                if (!tableQueries.ContainsKey(table))
                {
                    tableQueries[table] = new Dictionary<string, string>();
                }
                tableQueries[table][operation] = command;
            }
        }


        private void AddRecord(string tableName, Dictionary<string, object> parameters)
        {
            string insertCommand = tableQueries[tableName]["insert"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(insertCommand, connection);
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private void UpdateRecord(string tableName, Dictionary<string, object> parameters)
        {
            string updateCommandText = tableQueries[tableName]["update"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(updateCommandText, connection);
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }

                connection.Open();
                command.ExecuteNonQuery();
                Debug.WriteLine($"Executed SQL Command: {command.CommandText}");
            }
            RefreshData(tableName);
        }

        private void DeleteRecord(string tableName, object id)
        {
            string deleteCommandText = tableQueries[tableName]["delete"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(deleteCommandText, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
            RefreshData(tableName);
        }

        /*private void RefreshData(string tableName)
        {
            string selectCommand = tableQueries[tableName]["select"];
            SqlDataAdapter adapter = new SqlDataAdapter(selectCommand, connectionString);
            ds.Tables[tableName].Clear();
            adapter.Fill(ds, tableName);

            if (bsParent.DataSource != null && bsParent.DataSource is DataTable && ((DataTable)bsParent.DataSource).TableName == tableName)
            {
                dataGridViewParent.DataSource = bsParent;
            }
        }*/

        private void RefreshData(string tableName)
        {
            string selectCommand = tableQueries[tableName]["select"];
            SqlDataAdapter adapter = new SqlDataAdapter(selectCommand, connectionString);

            // Temporarily disable constraints
            ds.EnforceConstraints = false;

            ds.Tables[tableName].Clear();
            adapter.Fill(ds, tableName);

            // Re-enable constraints
            ds.EnforceConstraints = true;

            if (bsParent.DataSource != null && bsParent.DataSource is DataTable && ((DataTable)bsParent.DataSource).TableName == tableName)
            {
                dataGridViewParent.DataSource = bsParent;
            }
        }


        private void dataGridViewParent_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            currentEditedRow = e.RowIndex;
        }


        private void LoadTableNamesToListBox()
        {
            listBox1.Items.Clear();
            XDocument config = XDocument.Load("C:\\Users\\belde\\OneDrive\\Desktop\\SGBD\\Lab1\\Lab1\\FisierConfigurare.xml");

            // Extracting table names from the configuration file
            var tableNames = config.Element("Configuration")
                                   .Element("Tables")
                                   .Elements("Table")
                                   .Select(x => x.Attribute("name").Value);

            foreach (var tableName in tableNames)
            {
                listBox1.Items.Add(tableName);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTable = listBox1.SelectedItem.ToString();


            bsParent = new BindingSource { DataSource = ds.Tables[selectedTable] };
            dataGridViewParent.DataSource = bsParent;


            LoadRelatedTableData(selectedTable);
        }

        private void LoadRelatedTableData(string parentTable)
        {
            var relationships = ds.Relations.Cast<DataRelation>().Where(r => r.ParentTable.TableName == parentTable);

            if (relationships.Any())
            {
                var relation = relationships.First();
                string childTableName = relation.ChildTable.TableName;

                bsChild = new BindingSource { DataSource = bsParent, DataMember = relation.RelationName };
                dataGridViewChild.DataSource = bsChild;
            }
            else
            {
                dataGridViewChild.DataSource = null;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a table.");
                return;
            }

            selectedTable = listBox1.SelectedItem.ToString();
            DataTable table = ds.Tables[selectedTable];


            int newRowIdx = dataGridViewParent.Rows.Count - 2;
            DataGridViewRow newGridRow = dataGridViewParent.Rows[newRowIdx];

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            foreach (DataColumn column in table.Columns)
            {
                if (newGridRow.Cells[column.ColumnName].Value != null)
                {
                    parameters.Add($"@{column.ColumnName}", newGridRow.Cells[column.ColumnName].Value);
                }
                else
                {
                    parameters.Add($"@{column.ColumnName}", DBNull.Value);
                }
            }


            AddRecord(selectedTable, parameters);


            RefreshData(selectedTable);
        }



        private void button3_Click(object sender, EventArgs e)
        {
            selectedTable = listBox1.SelectedItem.ToString();

            if (dataGridViewParent.CurrentRow != null)
            {
                var id = dataGridViewParent.CurrentRow.Cells[0].Value;
                DeleteRecord(selectedTable, id);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (currentEditedRow.HasValue && listBox1.SelectedItem != null)
            {
                string selectedTable = listBox1.SelectedItem.ToString();
                var row = dataGridViewParent.Rows[currentEditedRow.Value];
                var id = row.Cells["id"].Value;
                Dictionary<string, object> parameters = new Dictionary<string, object>();

                foreach (DataGridViewColumn column in dataGridViewParent.Columns)
                {
                    parameters.Add($"@{column.Name}", row.Cells[column.Name].Value ?? DBNull.Value);
                }

                UpdateRecord(selectedTable, parameters);
                currentEditedRow = null;
            }
        }

        private void dataGridViewParent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewChild_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (selectedTable == "Clienti" && dataGridViewParent.CurrentRow != null)
            {
                int clientId1 = (int)dataGridViewParent.CurrentRow.Cells[0].Value;
                int clientId2 = clientId1 + 1; // Assuming the IDs are sequential

                // Start the deadlock simulation in a separate thread
                ThreadPool.QueueUserWorkItem(new WaitCallback((state) =>
                {
                    SimulateDeadlock(clientId1, clientId2);
                }));
            }
            else
            {
                MessageBox.Show("Please select a row from the Clienti table to simulate deadlock.");
            }
        }




        private void ExecuteDatabaseOperations(string queryType, int clientId1, int clientId2)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        SqlCommand command = connection.CreateCommand();
                        command.Transaction = transaction;

                        if (queryType == "UpdateFirstName")
                        {
                            command.CommandText = $"UPDATE Clienti SET nume = 'John' WHERE id = @clientId1";
                            command.Parameters.AddWithValue("@clientId1", clientId1);
                            command.ExecuteNonQuery();
                            Thread.Sleep(500); // Sleep to simulate delay for deadlock scenario
                            command.CommandText = $"UPDATE Clienti SET nume = 'Doe' WHERE id = @clientId2";
                            command.Parameters.AddWithValue("@clientId2", clientId2); // Use different parameter name
                            command.ExecuteNonQuery();
                        }
                        else if (queryType == "UpdateLastName")
                        {
                            command.CommandText = $"UPDATE Clienti SET nume = 'Doe' WHERE id = @clientId1";
                            command.Parameters.AddWithValue("@clientId1", clientId1);
                            command.ExecuteNonQuery();
                            Thread.Sleep(500); // Sleep to simulate delay for deadlock scenario
                            command.CommandText = $"UPDATE Clienti SET nume = 'John' WHERE id = @clientId2";
                            command.Parameters.AddWithValue("@clientId2", clientId2); // Use different parameter name
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 1205) // Deadlock
                {
                    Log("Deadlock encountered, retrying...");
                }
                else
                {
                    Log($"SQL Error: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Log($"Error: {ex.Message}");
            }
        }




        //Method to simulate deadlock scenario
        public void SimulateDeadlock(int clientId1, int clientId2)
        {
            // Execute two database operations in parallel with potential for deadlock
            Thread thread1 = new Thread(() => ExecuteDatabaseOperations("UpdateFirstName", clientId1, clientId2));
            Thread thread2 = new Thread(() => ExecuteDatabaseOperations("UpdateLastName", clientId1, clientId2));
            thread1.Start();
            thread2.Start();

            // Wait for both threads to finish
            thread1.Join();
            thread2.Join();
        }


        private void Log(string message)
        {
            string logPath = @"C:\Users\belde\OneDrive\Desktop\SGBD\Lab3\history.txt";
            try
            {
                using (StreamWriter writer = new StreamWriter(logPath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to write to log file: {ex.Message}");
            }
        }
    }
}