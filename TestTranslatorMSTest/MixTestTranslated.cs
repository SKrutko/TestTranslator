using Microsoft.VisualStudio.TestTools.UnitTesting;
using System; // one line comment
using System.Collections.Generic;
using System.Text;
/*multiple
 * line
 * comment*/
namespace TestTranslator
{
    //[TestClass]
    //[Owner("Sofia Krutko")]
    //public class TestMix
    //{
        //Document document;
        //[TestInitialize]
        //public void Setup()
        //{
            //document = new Document();
        //}
        //[TestMethod]
        //[Retry(3)]
        //public void TestCollectionAssert()
        //{
            //List<documentUnit> expected = new List<documentUnit>();
            //expected.Add(documentUnit.Using);
            //expected.Add(documentUnit.Using);
            //expected.Add(documentUnit.OneLineCommentAfterCode);
            //expected.Add(documentUnit.Using);
            //expected.Add(documentUnit.Namespace);
            //expected.Add(documentUnit.MultipleLineComment);
            //List<string> given = new List<string>();
            //given.Add("first line");
            //given.Add("second line");
            //document.addUsingStatement("System.Collections.Generic");
            //document.addUsingStatement("System.Text");
            //document.addComment("system.text comment", true);
            //document.addUsingStatement("System");
            //document.addNamespaceStatement("DocumentTests");
            //document.addComment(given, false);
            //CollectionAssert.AreEqual(expected, document.getDocumentStructure());
        //}
    //}
    [TestClass]
    public class TestMixClass2
    {
        CodeGenerator codeGenerator;
        [TestInitialize]
        [Description("is repeated before each test method")]
        public void Setup()
        {
            codeGenerator = new CodeGenerator();
        }
        [TestMethod]
        public void generate_givenEmptyDoc_returnOneUsing()
        {
            Document given = new Document();
            List<string> expected = new List<string>();
            Assert.AreEqual("using Microsoft.VisualStudio.TestTools.UnitTesting;", codeGenerator.TranslateDocument(given)[0]);
        }
    }
    [TestClass]
    public class TestMixClass3
    {
        // TextMexClass3 is without any class attribute
        Scanner scanner;
        [TestInitialize]
        public void Setup()
        {
            scanner = new Scanner();
        }
        [TestMethod]
        public void isSingleSpecialCar_givenSpecialCar_returnTrue()
        {
            Assert.IsTrue(scanner.isSingleSpecialCharacter('.'));
        }
    }
    //[TestClass]
    //[Description("should be commented")]
    //public class TestMixClass4
    //{
        //Parser parser;
    //}
}
