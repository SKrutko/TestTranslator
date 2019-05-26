using Microsoft.VisualStudio.TestTools.UnitTesting;
using System; // one line comment
using System.Collections.Generic;
using System.Text;
namespace TestTranslator
{
    [TestClass]
    public class DocumentTests
    {
        Document document;
        [TestInitialize]
        public void Setup()
        {
            document = new Document();
        }
        [TestMethod]
        public void getDocumentStructure_()
        {
            List<documentUnit> expected = new List<documentUnit>();
            expected.Add(documentUnit.Using);
            expected.Add(documentUnit.Using);
            expected.Add(documentUnit.OneLineCommentAfterCode);
            expected.Add(documentUnit.Using);
            expected.Add(documentUnit.Namespace);
            expected.Add(documentUnit.MultipleLineComment);
            List<string> given = new List<string>();
            given.Add("first line");
            given.Add("second line");
            document.addUsingStatement("System.Collections.Generic");
            document.addUsingStatement("System.Text");
            document.addComment("system.text comment", true);
            document.addUsingStatement("System");
            document.addNamespaceStatement("DocumentTests");
            document.addComment(given, false);
            CollectionAssert.AreEqual(expected, document.getDocumentStructure());
        }
    }
}

