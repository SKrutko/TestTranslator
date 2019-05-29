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
        private List<Attribute>listOfAttributes;
        bool commented = false;

        public Class(string className)
        {
            name = className;
            listOfAttributes = new List<Attribute>();
        }
        public Class(string className, List<Attribute> listOfAttributes)
        {
            name = className;
            this.listOfAttributes = listOfAttributes;
            foreach(Attribute a in listOfAttributes)
            {
                if (!a.IsTranslatable())
                    commented = true;
            }
        }
        public string getName()
        {
            return "MS" + name;
        }

        public List<Attribute> getListOfAttributes()
        {
            return listOfAttributes;
        }

        public bool IsCommented()
        {
            return commented;
        }

        
    }
}
