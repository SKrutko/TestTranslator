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
        private List<string> resultList;
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

            saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\";

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
        public void EnableButtonSave()
        {
            btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                string filePath = saveFileDialog1.FileName;
                System.IO.File.WriteAllLines(filePath, resultList);
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
        public void SetResultList(List<string> list)
        {
            resultList = list;
        }
    }
}
