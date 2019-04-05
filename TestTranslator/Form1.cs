using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestTranslator
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //specify openFileDialog
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "test files (*.cs)|*.cs";
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Title = "Please select a NUnit test file.";
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                lblStatusCheck.Text = "file is read";
                string filePath = openFileDialog.FileName;
                Program.createScanner(filePath);
            }
        }

        public void print(string line)
        {
            rtbMain.Text +=  line + "\n";
        }
    }
}
