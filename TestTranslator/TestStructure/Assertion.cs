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

        private Dictionary<string, string> MethodsTranslations;
        private List<string> TranslatableAssertions;
        private List<string> TranslatableMethods;

        public Assertion(string type)
        {
            this.type = type;
            fillInDictionary();
        }

        public void SetMethod(string method)
        {
            this.method = method;
        }

        public void SetArgs(string args)
        {
            this.args = args;
        }

        public bool IsTranslatable()
        {
            if (TranslatableAssertions.Contains(type)
                && (TranslatableMethods.Contains(method) || MethodsTranslations.ContainsKey(method)))
                return true;
            return false;
        }

        public string GetType()
        {
            return type;
        }
        public string GetTranslatedMethod()
        {
            string translatedMethod;
            MethodsTranslations.TryGetValue(method, out translatedMethod);
            if (MethodsTranslations.ContainsKey(method))
                return translatedMethod;
            return method;
        }
        public string GetArgs()
        {
            return args;
        }

        private void fillInDictionary()
        {
            MethodsTranslations = new Dictionary<string, string>();
            TranslatableAssertions = new List<string>();
            TranslatableMethods = new List<string>();

            MethodsTranslations.Add("True", "IsTrue");
            MethodsTranslations.Add("False", "IsFalse");
            MethodsTranslations.Add("Null", "IsNull");
            MethodsTranslations.Add("NotNull", "IsNotNull");

            TranslatableAssertions.Add("Assert");
            TranslatableAssertions.Add("StringAssert");
            TranslatableAssertions.Add("CollectionAssert");
            TranslatableMethods.Add("AreEqual");
            TranslatableMethods.Add("AreNotEqual");
            TranslatableMethods.Add("AreSame");
            TranslatableMethods.Add("AreNotSame");
            TranslatableMethods.Add("IsInstanceOf");
            TranslatableMethods.Add("IsNotInstanceOf");
            TranslatableMethods.Add("Pass");
            TranslatableMethods.Add("Fail");
            TranslatableMethods.Add("Ignore");
            TranslatableMethods.Add("Inconclusive");

            TranslatableMethods.Add("Contains");
            TranslatableMethods.Add("DoesNotContain");
            TranslatableMethods.Add("StartsWith");
            TranslatableMethods.Add("EndsWith");
            TranslatableMethods.Add("DoesNotMatch");

            TranslatableMethods.Add("AllItemsAreInstancesOfType");
            TranslatableMethods.Add("AllItemsAreNotNull");
            TranslatableMethods.Add("AllItemsAreUnique");
            TranslatableMethods.Add("AreEquivalent");
            TranslatableMethods.Add("AreNotEquivalent");
            TranslatableMethods.Add("DoesNotContain");
            TranslatableMethods.Add("Equals");
            TranslatableMethods.Add("IsNotSubsetOf");
            TranslatableMethods.Add("IsSubsetOf");
            TranslatableMethods.Add("ReferenceEquals");
        }

        public string GetLineToPrint()
        {
            return GetType() + "." + GetTranslatedMethod() + "(" + GetArgs() + ");";
        }
    }
}
