using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTranslator
{
    public class Class
    {
        private string name;

        public Class(string className)
        {
            name = className;
        }
        public string getName()
        {
            return name;
        }
    }
}
