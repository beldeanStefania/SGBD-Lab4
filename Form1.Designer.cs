namespace Lab3
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            bindingSource1 = new BindingSource(components);
            dataGridViewParent = new DataGridView();
            dataGridViewChild = new DataGridView();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            label1 = new Label();
            listBox1 = new ListBox();
            button4 = new Button();
            button5 = new Button();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewParent).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewChild).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewParent
            // 
            dataGridViewParent.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewParent.Location = new Point(166, 81);
            dataGridViewParent.Name = "dataGridViewParent";
            dataGridViewParent.RowHeadersWidth = 62;
            dataGridViewParent.RowTemplate.Height = 28;
            dataGridViewParent.Size = new Size(622, 331);
            dataGridViewParent.TabIndex = 0;
            dataGridViewParent.CellContentClick += dataGridViewParent_CellContentClick;
            dataGridViewParent.CellEndEdit += dataGridViewParent_CellEndEdit;
            // 
            // dataGridViewChild
            // 
            dataGridViewChild.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewChild.Location = new Point(1073, 81);
            dataGridViewChild.Name = "dataGridViewChild";
            dataGridViewChild.RowHeadersWidth = 62;
            dataGridViewChild.RowTemplate.Height = 28;
            dataGridViewChild.Size = new Size(613, 331);
            dataGridViewChild.TabIndex = 1;
            dataGridViewChild.CellContentClick += dataGridViewChild_CellContentClick;
            // 
            // button1
            // 
            button1.BackColor = SystemColors.ActiveBorder;
            button1.Location = new Point(1373, 467);
            button1.Name = "button1";
            button1.Size = new Size(119, 53);
            button1.TabIndex = 2;
            button1.Text = "Adauga";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = SystemColors.ActiveBorder;
            button2.Location = new Point(1373, 602);
            button2.Name = "button2";
            button2.Size = new Size(119, 53);
            button2.TabIndex = 3;
            button2.Text = "Modifica";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.BackColor = SystemColors.ActiveBorder;
            button3.Location = new Point(1373, 738);
            button3.Name = "button3";
            button3.Size = new Size(119, 53);
            button3.TabIndex = 4;
            button3.Text = "Sterge";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(104, 478);
            label1.Name = "label1";
            label1.Size = new Size(488, 42);
            label1.TabIndex = 5;
            label1.Text = "Introduceti tabel prelucrare";
            label1.Click += label1_Click;
            // 
            // listBox1
            // 
            listBox1.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 38;
            listBox1.Location = new Point(139, 568);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(243, 308);
            listBox1.TabIndex = 6;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // button4
            // 
            button4.BackColor = SystemColors.ActiveBorder;
            button4.Location = new Point(1373, 881);
            button4.Name = "button4";
            button4.Size = new Size(119, 53);
            button4.TabIndex = 7;
            button4.Text = "DeadLockSim";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.BackColor = SystemColors.ActiveBorder;
            button5.Location = new Point(1373, 990);
            button5.Name = "button5";
            button5.Size = new Size(119, 53);
            button5.TabIndex = 8;
            button5.Text = "Refresh";
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // Form1
            // 
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(1904, 1234);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(listBox1);
            Controls.Add(label1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(dataGridViewChild);
            Controls.Add(dataGridViewParent);
            Name = "Form1";
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewParent).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewChild).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        //private System.Windows.Forms.TextBox textBoxId;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.DataGridView dataGridViewParent;
        private System.Windows.Forms.DataGridView dataGridViewChild;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox1;
        private Button button4;
        private Button button5;
    }
}