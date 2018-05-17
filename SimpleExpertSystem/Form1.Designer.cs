namespace SimpleExpertSystem
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadKBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AnalizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InvertedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.KBListView = new System.Windows.Forms.ListView();
            this.ColumnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.ObjectsListView = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.HistoryListView = new System.Windows.Forms.ListView();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.AnalizeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(710, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadKBToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.FileToolStripMenuItem.Text = "Файл";
            // 
            // LoadKBToolStripMenuItem
            // 
            this.LoadKBToolStripMenuItem.Name = "LoadKBToolStripMenuItem";
            this.LoadKBToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.LoadKBToolStripMenuItem.Text = "Загрузить базу знаний...";
            this.LoadKBToolStripMenuItem.Click += new System.EventHandler(this.LoadKBToolStripMenuItem_Click);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.ExitToolStripMenuItem.Text = "Выход";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // AnalizeToolStripMenuItem
            // 
            this.AnalizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StartToolStripMenuItem,
            this.InvertedToolStripMenuItem});
            this.AnalizeToolStripMenuItem.Name = "AnalizeToolStripMenuItem";
            this.AnalizeToolStripMenuItem.Size = new System.Drawing.Size(114, 20);
            this.AnalizeToolStripMenuItem.Text = "Логический вывод";
            // 
            // StartToolStripMenuItem
            // 
            this.StartToolStripMenuItem.Name = "StartToolStripMenuItem";
            this.StartToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.StartToolStripMenuItem.Text = "Прямой...";
            this.StartToolStripMenuItem.Click += new System.EventHandler(this.StartToolStripMenuItem_Click);
            // 
            // InvertedToolStripMenuItem
            // 
            this.InvertedToolStripMenuItem.Name = "InvertedToolStripMenuItem";
            this.InvertedToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.InvertedToolStripMenuItem.Text = "Обратный...";
            this.InvertedToolStripMenuItem.Click += new System.EventHandler(this.InvertedToolStripMenuItem_Click);
            // 
            // KBListView
            // 
            this.KBListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1,
            this.ColumnHeader2});
            this.KBListView.FullRowSelect = true;
            this.KBListView.Location = new System.Drawing.Point(8, 19);
            this.KBListView.Name = "KBListView";
            this.KBListView.Size = new System.Drawing.Size(264, 161);
            this.KBListView.TabIndex = 1;
            this.KBListView.UseCompatibleStateImageBehavior = false;
            this.KBListView.View = System.Windows.Forms.View.Details;
            // 
            // ColumnHeader1
            // 
            this.ColumnHeader1.Text = "Свойство";
            this.ColumnHeader1.Width = 119;
            // 
            // ColumnHeader2
            // 
            this.ColumnHeader2.Text = "Значение";
            this.ColumnHeader2.Width = 141;
            // 
            // ObjectsListView
            // 
            this.ObjectsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.ObjectsListView.FullRowSelect = true;
            this.ObjectsListView.Location = new System.Drawing.Point(6, 19);
            this.ObjectsListView.Name = "ObjectsListView";
            this.ObjectsListView.Size = new System.Drawing.Size(673, 161);
            this.ObjectsListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.ObjectsListView.TabIndex = 3;
            this.ObjectsListView.UseCompatibleStateImageBehavior = false;
            this.ObjectsListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Объект";
            this.columnHeader3.Width = 298;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Значение";
            this.columnHeader4.Width = 370;
            // 
            // HistoryListView
            // 
            this.HistoryListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.HistoryListView.FullRowSelect = true;
            this.HistoryListView.Location = new System.Drawing.Point(6, 19);
            this.HistoryListView.Name = "HistoryListView";
            this.HistoryListView.Size = new System.Drawing.Size(673, 198);
            this.HistoryListView.TabIndex = 5;
            this.HistoryListView.UseCompatibleStateImageBehavior = false;
            this.HistoryListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Объект";
            this.columnHeader6.Width = 171;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Значение";
            this.columnHeader7.Width = 214;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Основание";
            this.columnHeader8.Width = 284;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ObjectsListView);
            this.groupBox1.Location = new System.Drawing.Point(12, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(685, 186);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Объекты базы знаний";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.KBListView);
            this.groupBox2.Location = new System.Drawing.Point(419, 28);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(278, 186);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Информация о базе знаний";
            this.groupBox2.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.HistoryListView);
            this.groupBox3.Location = new System.Drawing.Point(12, 220);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(685, 223);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "История логического вывода";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 456);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Механизм логического вывода экспертной системы";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadKBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.ListView KBListView;
        private System.Windows.Forms.ColumnHeader ColumnHeader1;
        private System.Windows.Forms.ColumnHeader ColumnHeader2;
        private System.Windows.Forms.ListView ObjectsListView;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ToolStripMenuItem AnalizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem StartToolStripMenuItem;
        private System.Windows.Forms.ListView HistoryListView;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ToolStripMenuItem InvertedToolStripMenuItem;
    }
}

