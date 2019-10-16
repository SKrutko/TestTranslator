using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTranslator
{
    class Dictionary
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private Dictionary<string, string> AttributesDictionary = new Dictionary<string, string>();
        private List<string> TranslatableMethodAttributes = new List<string>();
        private List<string> TranslatableClassAttributes = new List<string>();


        private List<string> TranslatableAssertionsMethods = new List<string>();
        private Dictionary<string, string> AssertionsMethodsDictionary = new Dictionary<string, string>();
        private List<string> TranslatableAssertions = new List<string>();

        public Dictionary()
        {
            FillInAttributesDictionary();
            FillInTranslatableMethodAttributes();
            FillInTranslatableClassAttributes();
            FillInTranslatableAssertions();
            FillInTranslatableAssertionsMethods();
            FillInAssertionsMethodsDictionary();
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

        private void FillInTranslatableMethodAttributes()
        {
            TranslatableMethodAttributes.Add("TestFixture");
            TranslatableMethodAttributes.Add("Test");
            TranslatableMethodAttributes.Add("Author");
            TranslatableMethodAttributes.Add("Category");
            TranslatableMethodAttributes.Add("Ignore");
            TranslatableMethodAttributes.Add("Description");
            TranslatableMethodAttributes.Add("Property");
        }

        private void FillInTranslatableClassAttributes()
        {
            TranslatableClassAttributes.Add("TestFixture");
            TranslatableClassAttributes.Add("Test");
            TranslatableClassAttributes.Add("Category");
            TranslatableClassAttributes.Add("Ignore");
        }

        private void FillInTranslatableAssertions()
        {
            TranslatableAssertions.Add("Assert");
            TranslatableAssertions.Add("CollectionAssert");
            TranslatableAssertions.Add("StringAssert");
        }

        private void FillInTranslatableAssertionsMethods()
        {
            TranslatableAssertionsMethods.Add("True");
            TranslatableAssertionsMethods.Add("IsTrue");
            TranslatableAssertionsMethods.Add("False");
            TranslatableAssertionsMethods.Add("IsFalse");
        }
        private void FillInAssertionsMethodsDictionary()
        {
            AssertionsMethodsDictionary.Add("True", "IsTrue");
            AssertionsMethodsDictionary.Add("False", "IsFalse");
        }

        public string TranslateAttributeName(string name)
        {
            String translatedName;
            AttributesDictionary.TryGetValue(name, out translatedName);
            return translatedName == null ? name : translatedName;
        }

        public bool IsTranslatableMethodAttribute(string attribute)
        {
            return TranslatableMethodAttributes.Contains(attribute);
        }

        public bool IsTranslatableClassAttribute(string attribute)
        {
            return TranslatableClassAttributes.Contains(attribute);
        }

        public bool IsTranslatable(AssertionExpression assertion)
        {
            if (!TranslatableAssertions.Contains(assertion.Class)) return false;
            return TranslatableAssertionsMethods.Contains( assertion.Method);
        }

        public AssertionExpression Translate(AssertionExpression assertion)
        {
            string translatedMethod;
            AssertionsMethodsDictionary.TryGetValue(assertion.Method, out translatedMethod);
            assertion.Method = translatedMethod == null ? assertion.Method : translatedMethod;
            Logger.Info("method : " + assertion.Method + ", translated method: " + translatedMethod);
            return assertion;
        }

    }
}
