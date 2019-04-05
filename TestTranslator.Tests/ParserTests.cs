using NUnit.Framework;
using System.Collections;
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
        public void parse_givenUsingStatement_resultAddedUsingStatementoDocument()
        {
            string expectedUsingStatement = "System.Collections";

            List<string> givenListOfTokens = new List<string>();
            givenListOfTokens.Add("using");
            givenListOfTokens.Add("System");
            givenListOfTokens.Add(".");
            givenListOfTokens.Add("Collections");
            givenListOfTokens.Add(";");
            Document document = Program.GetDocument();

            parser.parse(givenListOfTokens);
            List<UsingStatement> result = document.getListOfUsingStatements();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expectedUsingStatement, result[0].getStatement());
        }
        [Test]
        public void parse_givenUsingStatement_resultAddedUsingStatementoDocument2()
        {
            string expectedUsingStatement1 = "System.Collections";
            string expectedUsingStatement2 = "System.Collections.Generic";

            List<string> givenListOfTokens1 = new List<string>();
            givenListOfTokens1.Add("using");
            givenListOfTokens1.Add("System");
            givenListOfTokens1.Add(".");
            givenListOfTokens1.Add("Collections");
            givenListOfTokens1.Add(";");

            List<string> givenListOfTokens2 = new List<string>();
            givenListOfTokens2.Add("using");
            givenListOfTokens2.Add("System");
            givenListOfTokens2.Add(".");
            givenListOfTokens2.Add("Collections");
            givenListOfTokens2.Add(".");
            givenListOfTokens2.Add("Generic");
            givenListOfTokens2.Add(";");

            Document document = Program.GetDocument();
            parser.parse(givenListOfTokens1);
            parser.parse(givenListOfTokens2);

            List<UsingStatement> result = document.getListOfUsingStatements();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(expectedUsingStatement1, result[0].getStatement());
            Assert.AreEqual(expectedUsingStatement2, result[1].getStatement());
        }

        [Test]
        public void parse_givenUsingStatementsWithNUnit_resultWithoutNUsing()
        {
            string expectedUsingStatement1 = "System.Collections";
            string expectedUsingStatement2 = "System.Collections.Generic";
            
            Document document = Program.GetDocument();

            List<string> givenListOfTokens1 = new List<string>();
            givenListOfTokens1.Add("using");
            givenListOfTokens1.Add("System");
            givenListOfTokens1.Add(".");
            givenListOfTokens1.Add("Collections");
            givenListOfTokens1.Add(";");

            List<string> givenListOfTokens2 = new List<string>();
            givenListOfTokens2.Add("using");
            givenListOfTokens2.Add("System");
            givenListOfTokens2.Add(".");
            givenListOfTokens2.Add("Collections");
            givenListOfTokens2.Add(".");
            givenListOfTokens2.Add("Generic");
            givenListOfTokens2.Add(";");

            List<string> givenListOfTokens3 = new List<string>();
            givenListOfTokens3.Add("using");
            givenListOfTokens3.Add("NUnit");
            givenListOfTokens3.Add(".");
            givenListOfTokens3.Add("Framework");
            givenListOfTokens3.Add(";");
            List<UsingStatement> result = document.getListOfUsingStatements();

            parser.parse(givenListOfTokens1);
            parser.parse(givenListOfTokens2);
            parser.parse(givenListOfTokens3);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(expectedUsingStatement1, result[0].getStatement());
            Assert.AreEqual(expectedUsingStatement2, result[1].getStatement());
        }
/*[Test]
        public void parse_givenUsingStatementsWithNamespace_resultWithNamespace()
        {
            string expectedUsingStatement1 = "System.Collections";
            string expectedUsingStatement2 = "System.Collections.Generic";
            string expectedNamespace = "HelloName";

            Document document = Program.GetDocument();

            List<string> givenListOfTokens1 = new List<string>();
            givenListOfTokens1.Add("using");
            givenListOfTokens1.Add("System");
            givenListOfTokens1.Add(".");
            givenListOfTokens1.Add("Collections");
            givenListOfTokens1.Add(";");

            List<string> givenListOfTokens2 = new List<string>();
            givenListOfTokens2.Add("using");
            givenListOfTokens2.Add("System");
            givenListOfTokens2.Add(".");
            givenListOfTokens2.Add("Collections");
            givenListOfTokens2.Add(".");
            givenListOfTokens2.Add("Generic");
            givenListOfTokens2.Add(";");

            List<string> givenListOfTokens3 = new List<string>();
            givenListOfTokens3.Add("using");
            givenListOfTokens3.Add("NUnit");
            givenListOfTokens3.Add(".");
            givenListOfTokens3.Add("Framework");
            givenListOfTokens3.Add(";");

            List<string> givenListOfTokens4 = new List<string>();
            givenListOfTokens4.Add("namespace");
            givenListOfTokens4.Add("HelloName");
            
            List<UsingStatement> result = document.getListOfUsingStatements();
            string namespaceResult = document.getNamespaceStatement();

            parser.parse(givenListOfTokens1);
            parser.parse(givenListOfTokens2);
            parser.parse(givenListOfTokens3);
            parser.parse(givenListOfTokens4);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(expectedUsingStatement1, result[0].getStatement());
            Assert.AreEqual(expectedUsingStatement2, result[1].getStatement());
            Assert.AreEqual(expectedNamespace, namespaceResult);
            Assert.AreEqual(ParserState.FoundnamespaceNameExpectedLeftBrace, parser.getState());
        }*/

      /*[Test]
        public void analize_givenUsingStatement_createdUsingStatementInDocument()
        {
            string[] given = {"uisng", "System", ".", "Collections" , ";"};

            for (int i = 0; i < 5; i++)
                parser.analize(given[i]);

            Document document = Program.GetDocument();
            List < UsingStatement > result= document.getListOfUsingStatements();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("System.Collections", result[0].getStatement());


        }*/




    }
}