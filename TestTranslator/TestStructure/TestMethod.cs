using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTranslator
{
    public class TestMethod
    {
        private List<Attribute> listOfAttributes;
        private string returnType;
        private string name;
        private string args;
        private bool commented = false;

        public TestMethod(string type,  List<Attribute> attributes)
        {
            returnType = type;
            listOfAttributes = new List<Attribute>();
            listOfAttributes.AddRange(attributes);

            foreach (Attribute a in attributes)
                if (!a.IsTranslatable())
                    commented = true;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public string GetReturnType()
        {
            return returnType;
        }

        public string GetName()
        {
            return name;
        }

        public void AddArgs(string args)
        {
            this.args = args;
        }

        public string GetArgs()
        {
            return args;
        }
        public List<Attribute> getListOfAttributes()
        {
            return listOfAttributes;
        }

        public bool isCommented()
        {
            return commented;
        }

        public void ToComment()
        {
            commented = true;
        }


    }
}
