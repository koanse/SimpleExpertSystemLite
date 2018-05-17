using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SimpleExpertSystem
{
    public partial class Form3 : Form
    {
        public Form3(ArrayList al)
        {
            InitializeComponent();
            for(int i = 0; i < al.Count; i++)
                comboBox1.Items.Add(((KBObject)al[i]).Name);
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