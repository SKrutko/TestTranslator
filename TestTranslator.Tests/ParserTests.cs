using NUnit.Framework;
using System.Collections; //comment 1
using System.Collections.Generic;

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
        public void getTokenType_givenClassAttrWithotArg_returnsClassAttributeWithoutArg()
        {
            Assert.AreEqual(TokenType.ClassAttributeWithoutArg, parser.getTokenType("TestFixture"));
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
            parser.FoundNUnit();

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
        [Test]
        public void analize_givenCommentCharInString_detectedAsString() //???
        {
            parser.changeState(ParserState.ExpectedCodeLineOrTestMethod);

            List<documentUnit> expected = new List<documentUnit>();
            expected.Add(documentUnit.CodeLineInClass);
            
            string[] givenToken = { "string", "s", "=", "\"", "/*", "something", "*/", "\"", ";" }; //string s = "/* something */";
        
            bool[] givenSpace = { false, true, true, true, false, true, true, false, false };
            int numberOfGivenElements = 9;
            for (int i = 0; i < numberOfGivenElements - 2; i++)
                parser.analize(givenToken[i], givenSpace[i], i == numberOfGivenElements);

            Assert.AreEqual(ParserState.ExpectedContinuationOfCode, parser.getState());
            parser.analize(givenToken[7], givenSpace[7], 7 == numberOfGivenElements);
            parser.analize(givenToken[8], givenSpace[8], 8 == numberOfGivenElements);

            Document document = Program.GetDocument();
            List<documentUnit> result = document.getDocumentStructure();

            CollectionAssert.AreEqual(expected, result);
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
            Assert.AreEqual("MSClassName", resultList[0].getName());
            Assert.AreEqual(ParserState.FoundClassNameExpectedLeftBrace, parser.getState());
        }

        [Test]
        public void analize_givenClassAttribute_createdClassWithAttributeInDocument()
        {
            string[] given = { "[", "TestFixture", "]", "public", "class", "ClassName" };

            parser.changeState(ParserState.ExpectedClass);

            parser.analize(given[0], true, false);
            Assert.AreEqual(ParserState.ExpectedClassAttribute, parser.getState());
            parser.analize(given[1], true, false);
            Assert.AreEqual(ParserState.FoundClassAttrExpectedRightBracketOrComma, parser.getState());
            parser.analize(given[2], true, true);
            Assert.AreEqual(ParserState.ExpectedClass, parser.getState());

            parser.analize(given[3], true, false);
            parser.analize(given[4], true, false);
            parser.analize(given[5], true, true);

            Document document = Program.GetDocument();
            List<Attribute> result = document.getListOfClasses()[0].getListOfAttributes();

            Assert.AreEqual(0, result.Count);
        }
        [Test]
        public void analize_givenCodeLineInClass_addedToDoc()
        {
            string[] givenToken = { "parser", ".", "analize", "(" , "given", ")", ";" };
            bool[] givenSpace = { false, false, false, false, true, true, false };
            bool[] givenEndl = { false, false, false, false, false, false, true };

            parser.changeState(ParserState.ExpectedCodeLineOrTestMethod);

            for(int i = 0; i < 7; i++)
            {
                parser.analize(givenToken[i], givenSpace[i], givenEndl[i]);
            }

            Document document = Program.GetDocument();
            
            Assert.AreEqual(documentUnit.CodeLineInClass, document.getDocumentStructure()[0]);
            Assert.AreEqual(1, document.getListOfCodeLines().Count);
            Assert.AreEqual("parser.analize( given );", document.getListOfCodeLines()[0]);
        }

        [Test]
        public void analize_givenTwoCodeLineInClass_addedBothToDoc()
        {
            string[] givenToken = { "parser", ".", "analize", "(", "given", ")", ";",
            "parser", ".", "changeState", "(", ")", ";"
        };
            bool[] givenSpace = { false, false, false, false, true, true, false,
            false, false, false, false, false, false,};
            bool[] givenEndl = { false, false, false, false, false, false, true,
            false, false, false, false, false, true};

            parser.changeState(ParserState.ExpectedCodeLineOrTestMethod);

            for (int i = 0; i < 13; i++)
            {
                parser.analize(givenToken[i], givenSpace[i], givenEndl[i]);
            }

            Document document = Program.GetDocument();

            Assert.AreEqual(documentUnit.CodeLineInClass, document.getDocumentStructure()[0]);
            Assert.AreEqual(documentUnit.CodeLineInClass, document.getDocumentStructure()[1]);
            Assert.AreEqual(2, document.getListOfCodeLines().Count);
            Assert.AreEqual("parser.analize( given );", document.getListOfCodeLines()[0]);
            Assert.AreEqual("parser.changeState();", document.getListOfCodeLines()[1]);

        }

        [Test]
        public void analize_givenTestMethod_createdTestMethodInDocument()
        {
            string[] givenToken = { "[", "SetUp", "]",
                "public", "void", "SetupMethod", "(", ")",
                "{",
                "}",
                "[", "Test", "]",
                "public", "void", "TestMethod", "(", ")",
                "{",
                "}" };
            bool[] givenSpace = { false, false, false,
                false, true, true, false, false,
                false,
                false,
                false, false, false,
                false, true, true, false, false,
                false,
                false};
            bool[] givenEndl = { false, false, true,
                false, false, false, false, true,
                true,
                true,
                false, false, true,
                false, false, false, false, true,
                true,
                true};

            parser.changeState(ParserState.ExpectedCodeLineOrTestMethod);

            for (int i = 0; i < 20; i++)
            {
                parser.analize(givenToken[i], givenSpace[i], givenEndl[i]);
            }

            Document document = Program.GetDocument();

            List<documentUnit> expectedStructure = new List<documentUnit>();
            expectedStructure.Add(documentUnit.TestAttributeWithoutArgs);
            expectedStructure.Add(documentUnit.TestMethodDeclaration);
            expectedStructure.Add(documentUnit.TestAttributeWithoutArgs);
            expectedStructure.Add(documentUnit.TestMethodDeclaration);

            List<TestMethod> result =  document.GetTestMethods();

            List<Attribute> attr1 = new List<Attribute>();
            attr1.Add(new Attribute(AttributeType.TestAttribute, "SetUp"));

            TestMethod tm1 = new TestMethod("void", attr1);
            tm1.AddArgs("");
            tm1.SetName("SetupMethod");

            List<Attribute> attr2 = new List<Attribute>();
            attr2.Add(new Attribute(AttributeType.TestAttribute, "Test"));

            TestMethod tm2 = new TestMethod("void", attr2);
            tm2.AddArgs("");
            tm2.SetName("TestMethod");

            List<TestMethod> expected = new List<TestMethod>();
            expected.Add(tm1);
            expected.Add(tm2);



            CollectionAssert.AreEqual(expectedStructure, document.getDocumentStructure() );

            Assert.AreEqual("TestInitialize", result[0].getListOfAttributes()[0].getKeyWord());
            Assert.AreEqual("TestMethod", result[1].getListOfAttributes()[0].getKeyWord());

           // CollectionAssert.AreEqual(attr1, result[0].getListOfAttributes());
            //CollectionAssert.AreEqual(attr2, result[1].getListOfAttributes());

            //CollectionAssert.AreEqual(expected, result);
        }
        [Test]
        public void analize_stringMode()
        {
            parser.changeState(ParserState.ExpectedContinuationOfCode);

            string[] givenToken = { "analize", "(", "\"", "heloo", "\"", ")", ";" };
            bool[] givenSpace = { false, false, false, false, false, false, false };
            bool[] givenEndl = { false, false, false, false, false, false, true };

            bool[] expectedStringMode = { false, false, true, true, false, false, false };

            for (int i = 0; i < 7; i++)
            {
                parser.analize(givenToken[i], givenSpace[i], givenEndl[i]);
                Assert.AreEqual(expectedStringMode[i], parser.GetStringMode());
            }
        }

        [Test]
        public void analize_EndlInString()
        {
            parser.changeState(ParserState.ExpectedContinuationOfCode);

            string[] givenToken = { "analize", "(", "\"", ";", "\"", ")", ";" };
            bool[] givenSpace = { false, false, false, false, false, false, false };
            bool[] givenEndl = { false, false, false, false, false, false, true };

            string expectedLine = "analize(\";\");";
            Document document = Program.GetDocument();


            for (int i = 0; i < 7; i++)
            {
                parser.analize(givenToken[i], givenSpace[i], givenEndl[i]);
            }

            List<string> expected = new List<string>();
            expected.Add(expectedLine);
            CollectionAssert.AreEqual(expected, document.getListOfCodeLines());

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
        [Test]
        public void analize_givenMLComment_addedToDocument()
        {
            string[] given = {"/*", "multiple",
            "*", "line",
            "*", "comment", "*/"};
            bool[] givenSpace = {false, true,
            false, true,
            false, true, false};
            bool[] givenEndl = {false, true,
            false, true,
            false, false, true};

            List<string> expected = new List<string>();
            expected.Add(" multiple");
            expected.Add("* line");
            expected.Add("* comment");


            for (int i = 0; i < 7; i++)
                parser.analize(given[i], givenSpace[i], givenEndl[i]);

            Document document = Program.GetDocument();

            Assert.AreEqual(documentUnit.MultipleLineComment, document.getDocumentStructure()[0]);
            CollectionAssert.AreEqual(expected, document.getListOfMultipleLineComments()[0].getMessage());
        }




    }

    }