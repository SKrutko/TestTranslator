using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTranslator
{
    public class Assertion
    {
        private string type;
        private string method;
        private string args;

        public Assertion(string type)
        {
            this.type = type;
        }

        public void SetMethod(string method)
        {
            this.method = method;
        }

        public void SetArgs(string args)
        {
            this.args = args;
        }

        public bool translatable()
        {
            return false; //TODO
        }

        public string GetTranslatedType()
        {
            //TODO
            return type;
        }
        public string GetTranslatedMethod()
        {
            //TODO
            return method;
        }
        public string GetTranslatedArgs()
        {
            //TODO
            return args;
        }

        public string GetLineToPrint()
        {
            return (GetTranslatedType() + "." + GetTranslatedMethod() + "(" + GetTranslatedArgs() + ");");
        }
    }
}
