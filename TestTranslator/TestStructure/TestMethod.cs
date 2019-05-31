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
        private bool classInitializeMethod = false;

        public TestMethod(string type,  List<Attribute> attributes)
        {
            returnType = type;
            listOfAttributes = new List<Attribute>();
            listOfAttributes.AddRange(attributes);

            foreach (Attribute a in attributes)
            {
                if (!a.IsTranslatable())
                    commented = true;

                //ClassInitialize should be public static void with single argument of type TestContext
                if (a.getKeyWord().Equals("ClassInitialize"))
                {
                    returnType = "static void";
                    SetClassInitializeMethod();
                }
                else if (a.getKeyWord().Equals("ClassCleanup"))
                {
                    returnType = "static void";
                }
            }
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
            return "MS" + name;
        }

        public void AddArgs(string args)
        {
            this.args = args;
        }

        public string GetArgs()
        {
            if(IsClassInitializeMethod())
                args = "TestContext context";
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
        private bool IsClassInitializeMethod()
        {
            return classInitializeMethod;
        }
        private void SetClassInitializeMethod()
        {
            classInitializeMethod = true;
        }


    }
}
