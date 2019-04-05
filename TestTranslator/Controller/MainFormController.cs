using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestTranslator
{
    public class MainFormController
    {
        private FormMain form;
        public MainFormController(FormMain _form)
        {
            this.form = _form;
        }

        public FormMain getForm()
        {
            return this.form;
        }

        public void printLine(string text)
        {
            form.print(text);
        }
    }
}
