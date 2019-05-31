using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTranslator
{
    public class Attribute
    {
        private AttributeType type;
        private string keyWord;
        private string arguments;
        private Dictionary<string, string> keyWords;

        public Attribute(AttributeType type, string keyWord)
        {
            this.type = type;
            this.keyWord = keyWord;
            fillInDictionary();
            arguments = "";
        }

        
        public string getKeyWord()
        {
            return translate();
        }

        public bool IsTranslatable()
        {
            if ((keyWord.Equals("Description") || keyWord.Equals("Author") || keyWord.Equals("Property"))
                && type == AttributeType.ClassAttribute)
                return false;
            return keyWords.ContainsKey(keyWord);
        }

        private void fillInDictionary()
        {
            keyWords = new Dictionary<string, string>();
            keyWords.Add("Test", "TestMethod");
            keyWords.Add("TestFixture", "TestClass");
            keyWords.Add("SetUp", "TestInitialize");
            keyWords.Add("TearDown", "TestCleanup");
            keyWords.Add("Author", "Owner");
            keyWords.Add("Category", "TestCategory");
            keyWords.Add("OneTimeSetUp", "ClassInitialize");
            keyWords.Add("OneTimeTearDown", "ClassCleanup");
            keyWords.Add("Property", "TestProperty");
            keyWords.Add("Description", "Description");
            keyWords.Add("Ignore", "Ignore");

        }
        private string translate()
        {
            string MSequivalent;
            keyWords.TryGetValue(keyWord, out MSequivalent);
            if (keyWords.ContainsKey(keyWord))
                return MSequivalent;
            return keyWord;
        }

        public void SetArguments(string args)
        {
            arguments = args;
        }

        public string GetArguments()
        {
            return arguments;
        }
        
    }
    public enum AttributeType
    {
        ClassAttribute,
        TestAttribute
    }


}
