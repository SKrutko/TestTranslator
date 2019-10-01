using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTranslator
{
    class Dictionary
    {
        private Dictionary<string, string> AttributesDictionary = new Dictionary<string, string>();
        private List<string> SimplyTranslatableAttributes = new List<string>();

        public Dictionary()
        {
            FillInAttributesDictionary();
            FillInSimplyTranslatableAttributes();
        }

        private void FillInAttributesDictionary()
        {
            AttributesDictionary.Add("Test", "TestMethod");
            AttributesDictionary.Add("TestFixture", "TestClass");
            AttributesDictionary.Add("SetUp", "TestInitialize");
            AttributesDictionary.Add("TearDown", "TestCleanup");
            AttributesDictionary.Add("Author", "Owner");
            AttributesDictionary.Add("Category", "TestCategory");
            AttributesDictionary.Add("OneTimeSetUp", "ClassInitialize");
            AttributesDictionary.Add("OneTimeTearDown", "ClassCleanup");
            AttributesDictionary.Add("Property", "TestProperty");
            AttributesDictionary.Add("Description", "Description");
            AttributesDictionary.Add("Ignore", "Ignore");
        }

        private void FillInSimplyTranslatableAttributes()
        {
            SimplyTranslatableAttributes.Add("TestFixture");
            SimplyTranslatableAttributes.Add("Test");
            SimplyTranslatableAttributes.Add("Author");
            SimplyTranslatableAttributes.Add("Category");
            SimplyTranslatableAttributes.Add("Ignore");
            SimplyTranslatableAttributes.Add("Description");
            SimplyTranslatableAttributes.Add("Property");
            SimplyTranslatableAttributes.Add("Theory");
        }

        public string TranslateAttributeName(string name)
        {
            String translatedName;
            AttributesDictionary.TryGetValue(name, out translatedName);
            return translatedName == null ? name : translatedName;
        }

        public bool IsSimplyTranslatableAttribute(string attribute)
        {
            return SimplyTranslatableAttributes.Contains(attribute);
        }
    }
}
