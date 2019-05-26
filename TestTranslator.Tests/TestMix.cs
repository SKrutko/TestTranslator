using System; // one line comment
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
/*multiple
 * line
 * comment*/

namespace TestTranslator
{
    [TestFixture, Author("Sofia Krutko")]
    class TestMix
    {
        Document document;

        [SetUp]
        public void Setup()
        {
            document = new Document();
        }

        [Test]
        [Retry(3)]
        public void TestCollectionAssert()
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


    [TestFixture]
    class TestMixClass2
    {
        CodeGenerator codeGenerator;
        [SetUp, Description("is repeated before each test method")]
        public void Setup()
        {
            codeGenerator = new CodeGenerator();
        }

        [Test]
        public void generate_givenEmptyDoc_returnOneUsing()
        {
            Document given = new Document();
            List<string> expected = new List<string>();
            Assert.AreEqual("using Microsoft.VisualStudio.TestTools.UnitTesting;", codeGenerator.TranslateDocument(given)[0]);
        }
    }
    class TestMixClass3
    {
        //TextMexClass3 is without any class attribute
        Scanner scanner;
        [SetUp]
        public void Setup()
        {
            scanner = new Scanner();
        }
        [Test]
        public void isSingleSpecialCar_givenSpecialCar_returnTrue()
        {
            Assert.True(scanner.isSingleSpecialCharacter('.'));
        }
    }

    [Description("should be commented")]
    class TestMixClass4
    {
        Parser parser;
    }

}
