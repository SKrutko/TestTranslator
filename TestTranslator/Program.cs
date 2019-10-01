using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics; // System.Start
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestTranslator
{
    public static class Program
    {
        static MainFormController mainFormController;
        static Document document;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainFormController = new MainFormController(new FormMain());
            Application.Run(mainFormController.getForm());
        }

        public static void createDocument()
        {
            document = new Document();
        }

        public static void createScanner(string filePath)
        {
            //read file
            try
            {
                using (FileStream fs = File.Open(filePath, FileMode.Open))
                {
                    //Scanner scanner = new Scanner(fs);
                    //scanner.scan();
                    NewScanner scanner = new NewScanner(fs);
                    scanner.scan();
                }
            }
            catch (Exception ex)//SecurityException ex)
            {
                MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                $"Details:\n\n{ex.StackTrace}");
            }
        }

        public static MainFormController getMainFormController()
        {
            return mainFormController;
        }

        public static Document GetDocument()
        {
            return document;
        }


    }
}
