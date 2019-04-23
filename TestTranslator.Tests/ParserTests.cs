using NUnit.Framework;
using System.Collections; //comment 1
using System.Collections.Generic;

/* multiple
 * line
 * comment
 * */

namespace TestTranslator
{
    public class ParserTests
    {
        Parser parser;
        [SetUp]
        public void Setup()
        {
            parser = new Parser();
        }

        [Test]
        public void getTokenType_givenUsing_returnsTypeUsing()
        {
            TokenType result = parser.getTokenType("using");
            Assert.That(TokenType.Using == result);
        }

        [Test]
        public void getTokenType_givenHello_returnsTypeNonToken()
        {
            TokenType result = parser.getTokenType("hello");
            Assert.That(TokenType.NonToken == result);
        }

        [Test]
        public void getTokenType_givenNamespace_returnsTypeNamespace()
        {
            TokenType result = parser.getTokenType("namespace");
            Assert.AreEqual(TokenType.Namespace, result);
        }

        [Test]
        public void getTokenType_givenTest_returnsTypeAttribute()
        {
            TokenType result = parser.getTokenType("Test");
            Assert.AreEqual(TokenType.Attribute, result);
        }

       
    }

    public class ParseTest
    {
        
        Parser parser;

        [SetUp]
        public void Setup()
        {
            Program.createDocument();
            parser = new Parser();
        }


      [Test]
        public void analize_givenUsingStatement_createdUsingStatementInDocument()
        {
            string[] givenToken = {"using", "System", ".", "Collections" , ";"};
            bool[] givenSpace = { false, true, false, false, false };
            int numberOfGivenElements = 5;
            for (int i = 0; i < numberOfGivenElements; i++)
                parser.analize(givenToken[i], givenSpace[i], i == numberOfGivenElements - 1);



            Document document = Program.GetDocument();
            List < UsingStatement > result= document.getListOfUsingStatements();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("System.Collections", result[0].getStatement());
        }

        [Test]
        public void analize_givenMultipleUsingStatements_createdListofUsingStatementsInDocument()
        {
            string[] givenTokens = { "using", "System", ".", "Collections", ";",
                "using", "NUnit", ".", "Framework", ";",
                "using", "System", ".", "Collections", ".", "Generic", ";",
            };
            bool[] givenSpaces = {false, true, false, false, false,
            false, true, false, false, false,
            false, true, false, false, false, false, false};
            bool[] givenEndls = { false, false, false, false, true,
            false, false, false, false, true,
            false, false, false, false, false, false, true};

            for (int i = 0; i < 17; i++)
                parser.analize(givenTokens[i], givenSpaces[i], givenEndls[i]);

            Document document = Program.GetDocument();
            List<UsingStatement> result = document.getListOfUsingStatements();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("System.Collections", result[0].getStatement());
            Assert.AreEqual("System.Collections.Generic", result[1].getStatement());
        }

        [Test]
        public void analize_givenUsingStAndNamespace_createdNamespaceInDocument()
        {
            string[] givenTokens = { "using", "System", ".", "Collections", ";",
                "using", "NUnit", ".", "Framework", ";",
                "using", "System", ".", "Collections", ".", "Generic", ";",
                "namespace", "HelloName", ".", "Collections", "{"
            };
            bool[] givenSpaces = {false, true, false, false, false,
            false, true, false, false, false,
            false, true, false, false, false, false, false,
            false, true, false, false, false,};
            bool[] givenEndls = { false, false, false, false, true,
            false, false, false, false, true,
            false, false, false, false, false, false, true,
            false, false, false, false, true,};

            for (int i = 0; i < 22; i++)
                parser.analize(givenTokens[i], givenSpaces[i], givenEndls[i]);

            Document document = Program.GetDocument();
            List<UsingStatement> result = document.getListOfUsingStatements();
            string resultName = document.getNamespaceStatement();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("System.Collections", result[0].getStatement());
            Assert.AreEqual("System.Collections.Generic", result[1].getStatement());
            Assert.AreEqual("HelloName.Collections", resultName);
            Assert.AreEqual(ParserState.ExpectedClass, parser.getState());
        }

        [Test]
        public void analize_givenNamespace_createdNamespaceInDocument()
        {
            string[] givenToken = { "namespace", "Namespace", ".", "Name", "{"};
            bool[] givenSpace = { false, true, false, false, false };
            int numberOfGivenElements = 5;

            for (int i = 0; i < numberOfGivenElements; i++)
                parser.analize(givenToken[i], givenSpace[i], i == numberOfGivenElements - 1);

            Document document = Program.GetDocument();
            string result = document.getNamespaceStatement();

            Assert.AreEqual("Namespace.Name", result);
            Assert.AreEqual(ParserState.ExpectedClass, parser.getState());
        }
    }
    class ParserTestClasses
    {
        Parser parser;
        [SetUp]
        public void Setup()
        {
            Program.createDocument();
            parser = new Parser();
        }

        [Test]
        public void analize_givenCWPublicWhenExpectedClass_changeState()
        {
            parser.changeState(ParserState.ExpectedClass);
            parser.analize("public", false, false);

            Assert.AreEqual(ParserState.ExpectedCWClass, parser.getState());
        }

        [Test]
        public void analize_givenCWClassWhenExpectedClass_changeState()
        {
            parser.changeState(ParserState.ExpectedClass);
            parser.analize("class", true, false);

            Assert.AreEqual(ParserState.FoundCWClassExpectedClassName, parser.getState());
        }
        [Test]
        public void analize_givenCWPublicClassWhenExpectedClass_changeState()
        {
            parser.changeState(ParserState.ExpectedClass);
            parser.analize("public", false, false);
            parser.analize("class", true, false);

            Assert.AreEqual(ParserState.FoundCWClassExpectedClassName, parser.getState());
        }

        [Test]
        public void analize_givenClass_createdClassInDocument()
        {
            string[] given = { "public", "class", "ClassName" };

            parser.changeState(ParserState.ExpectedClass);
            for (int i = 0; i < 3; i++)
                parser.analize(given[i], true, false);

            Document document = Program.GetDocument();
            List<Class> resultList = document.getListOfClasses();

            Assert.AreEqual(1, resultList.Count);
            Assert.AreEqual("ClassName", resultList[0].getName());
            Assert.AreEqual(ParserState.FoundClassNameExpectedLeftBrace, parser.getState());
        }

    }

    [TestFixture]
    class ParserCommentTests
    {
        Parser parser;
        [SetUp]
        public void Setup()
        {
            Program.createDocument();
            parser = new Parser();
        }

        [Test]
        public void getTokenType_oneLineComment_returnTypeComment()
        {
            Assert.AreEqual(TokenType.Comment, parser.getTokenType("//"));
            Assert.AreEqual(TokenType.Comment, parser.getTokenType("/*"));
        }

        [Test]
        public void analize_givenUsingWithComment_createdInDoc()
        {
            string[] givenToken = { "using", "//" , "this", "is", "comment",
                "System", ".", "Collections", ";"};
            bool[] givenSpace = { false, false, true, true, true,
                true, false, false, false};
            int numberOfGivenElements = 9;
            for (int i = 0; i < numberOfGivenElements; i++)
                parser.analize(givenToken[i], givenSpace[i], i == 4);



            Document document = Program.GetDocument();
            List<UsingStatement> result = document.getListOfUsingStatements();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("System.Collections", result[0].getStatement());
           
        }

        [Test]
        public void analize_givenUsingWithMLComment_createdInDoc()
        {
            List<documentUnit> expected = new List<documentUnit>();
            expected.Add(documentUnit.MultipleLineComment);

            string[] givenToken = { "/*" , "this", "is", "comment",
                "System", ".", "Collections", "*/"};
            bool[] givenSpace = { true, true, true, true, true,
                true, true, true,};
            int numberOfGivenElements = 8;
            for (int i = 0; i < numberOfGivenElements; i++)
                parser.analize(givenToken[i], givenSpace[i], i == 3);



            Document document = Program.GetDocument();
            List<documentUnit> result = document.getDocumentStructure();

            CollectionAssert.AreEqual(expected, result);
            //Assert.True(true);

        }

        [Test]
        public void analize_givenUsingWithMLComment_changeState()
        {
            parser.changeState(ParserState.ExpectedClass);
            parser.analize("/*", true, true);

            Assert.AreEqual(ParserState.MultipleLineComment, parser.getState());

            parser.analize("*/", true, true);
            Assert.AreEqual(ParserState.ExpectedClass, parser.getState());
        }



    }
}