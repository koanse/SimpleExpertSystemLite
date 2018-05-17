using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SimpleExpertSystem
{
    public partial class Form2 : Form
    {
        public Form2(string ObjectName, string[] LegalValues, string Comment)
        {
            InitializeComponent();
            label1.Text += "'" + ObjectName + "':";
            comboBox1.Items.AddRange(LegalValues);
            textBox1.Text = Comment;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }


    }
}