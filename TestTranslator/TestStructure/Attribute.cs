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
        private Dictionary<string, string> keyWords;

        public Attribute(AttributeType type, string keyWord)
        {
            this.type = type;
            this.keyWord = keyWord;
            fillInDictionary();
        }

        public AttributeType getType()
        {
            return type;
        }

        public string getKeyWord()
        {
            return translate(keyWord);
        }

        public bool IsTranslatable()
        {
            return true;
        }

        private void fillInDictionary()
        {
            keyWords = new Dictionary<string, string>();
            keyWords.Add("Test", "TestCase");
            keyWords.Add("TestFixture", "TestClass");
            keyWords.Add("SetUp", "TestInitialize");
            keyWords.Add("TearDown", "TestCleanUp");
            keyWords.Add("Author", "Owner");
            keyWords.Add("Category", "TestCategory");
            keyWords.Add("OneTimeSetUp", "ClassInitialize");
            keyWords.Add("OneTimeTearDown", "ClassCleanup");
            keyWords.Add("Property", "TestProperty");
            keyWords.Add("Theory", "DataRow");
        }
        private string translate(string kw)
        {
            string MSequivalent;
            keyWords.TryGetValue(kw, out MSequivalent);
            if (keyWords.ContainsKey(kw))
                return MSequivalent;
            return kw;
        }
    }

    public enum AttributeType
    {
        ClassAttribute,
        TestAttribute
    }
}
