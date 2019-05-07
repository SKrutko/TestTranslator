using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;

namespace TestTranslator
{
    class ScannerTests
    {
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
            Assert.True(scanner.isSingleSpecialCharacter(','));
            Assert.True(scanner.isSingleSpecialCharacter(';'));
            Assert.True(scanner.isSingleSpecialCharacter(':'));

        }

        [Test]
        public void getListOfTokens_givenNothing_returnEmptyList()
        {
            string given = "";
            List<scannerResponse> expected = new List<scannerResponse>();
            List<scannerResponse> result = scanner.getListOfTokens(given);
            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void getListOfTokens_givenOneWord_returnListWithOneElement()
        {
            string given = "using";
            List<scannerResponse> expected = new List<scannerResponse>();
            expected.Add(new scannerResponse("using", false));

            List<scannerResponse> result = scanner.getListOfTokens(given);
            Assert.IsTrue(true);
            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void getListOfTokens_givenTwoWords_returnListWithTwoElements()
        {
            string given = "using System";
            List<scannerResponse> expected = new List<scannerResponse>();
            expected.Add(new scannerResponse("using", false));
            expected.Add(new scannerResponse("System", true));

            List<scannerResponse> result = scanner.getListOfTokens(given);

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void getListOfTokens_givenTwoWordsWithSpaces_returnListWithTwoElements()
        {
            string given = "using   System   ";
            List<scannerResponse> expected = new List<scannerResponse>();
            expected.Add(new scannerResponse("using", false));
            expected.Add(new scannerResponse("System", true));

            List<scannerResponse> result = scanner.getListOfTokens(given);

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void getListOfTokens_givenWordsWithSpecialChar_returnList()
        {
            string given = "using System.Framework";
            List<scannerResponse> expected = new List<scannerResponse>();
            expected.Add(new scannerResponse("using", false));
            expected.Add(new scannerResponse("System", true));
            expected.Add(new scannerResponse(".", false));
            expected.Add(new scannerResponse("Framework", false));

            List<scannerResponse> result = scanner.getListOfTokens(given);

            CollectionAssert.AreEqual(expected, result);
        }

       [Test]
        public void getListOfTokens_givenWordsWithSpecialChar_returnList2()
        {
            string given = "using System.Framework;";
            List<scannerResponse> expected = new List<scannerResponse>();
            expected.Add(new scannerResponse("using", false));
            expected.Add(new scannerResponse("System", true));
            expected.Add(new scannerResponse(".", false));
            expected.Add(new scannerResponse("Framework", false));
            expected.Add(new scannerResponse(";", false));
            List<scannerResponse> result = scanner.getListOfTokens(given);

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void getListOfTokens_givenComment_returnList()
        {
            string given = " //";
            List<scannerResponse> expected = new List<scannerResponse>();
            expected.Add(new scannerResponse("//", true));

            List<scannerResponse> result = scanner.getListOfTokens(given);

            CollectionAssert.AreEqual(expected, result);

        }

        [Test]
        public void getListOfTokens_givenSpecialChar_returnList()
        {
            string given = "// aaa;a /* aaaaaa*/ aaaa";
            List<scannerResponse> expected = new List<scannerResponse>();
            expected.Add(new scannerResponse("//", false));
            expected.Add(new scannerResponse("aaa", true));
            expected.Add(new scannerResponse(";", false));
            expected.Add(new scannerResponse("a", false));
            expected.Add(new scannerResponse("/*", true));
            expected.Add(new scannerResponse("aaaaaa", true));
            expected.Add(new scannerResponse("*/", false));
            expected.Add(new scannerResponse("aaaa", true));

            List<scannerResponse> result = scanner.getListOfTokens(given);

            Assert.AreEqual(false, result[0].space);
            Assert.AreEqual(true, result[1].space);
            Assert.AreEqual(false, result[2].space);
            Assert.AreEqual(false, result[3].space);
            Assert.AreEqual(true, result[4].space);
            Assert.AreEqual(true, result[5].space);
            Assert.AreEqual(false, result[6].space);
            Assert.AreEqual(true, result[7].space);
            CollectionAssert.AreEqual(expected, result);

        }

        [Test]
        public void isSpecialToken_givenSpecialToken_returnTrue()
        {
            Assert.IsTrue(scanner.isSpecialToken("//"));
            Assert.IsTrue(scanner.isSpecialToken("*/"));
            Assert.IsTrue(scanner.isSpecialToken("/*"));
        }

        [Test]
        public void isSpecialToken_givenNotSpecialToken_returnFalse()
        {
            Assert.IsFalse(scanner.isSpecialToken("/a"));
            Assert.IsFalse(scanner.isSpecialToken("hgnss"));
            Assert.IsFalse(scanner.isSpecialToken("**"));
            Assert.IsFalse(scanner.isSingleSpecialCharacter('*'));
        }

        [Test]
        public void isDoubleSpecialToken_givenDoubleSpecTok_returnTrue()
        {
            Assert.IsTrue(scanner.isDoubleSpecialCharacter('/'));
            Assert.IsTrue(scanner.isDoubleSpecialCharacter('*'));

        }

        [Test]
        public void getListOfTokens_givenSpecialChar_returnList2()
        {
            string given = "/*//aaa;a/*aaaaaa*/;aaaa";
            List<scannerResponse> expected = new List<scannerResponse>();
            expected.Add(new scannerResponse("/*", false));
            expected.Add(new scannerResponse("//", false));
            expected.Add(new scannerResponse("aaa", false));
            expected.Add(new scannerResponse(";", false));
            expected.Add(new scannerResponse("a", false));
            expected.Add(new scannerResponse("/*", false));
            expected.Add(new scannerResponse("aaaaaa", false));
            expected.Add(new scannerResponse("*/", false));
            expected.Add(new scannerResponse(";", false));
            expected.Add(new scannerResponse("aaaa", false));

            List<scannerResponse> result = scanner.getListOfTokens(given);

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void getListOfTokens_givenSpecialChar_returnList3()
        {
            string given = "            expected.Add(\"    public class CodeGeneratorTests\");";
            List<scannerResponse> expected = new List<scannerResponse>();
            expected.Add(new scannerResponse("expected", true));
            expected.Add(new scannerResponse(".", false));
            expected.Add(new scannerResponse("Add", false));
            expected.Add(new scannerResponse("(", false));
            expected.Add(new scannerResponse("\"", false));
            expected.Add(new scannerResponse("public", true));
            expected.Add(new scannerResponse("class", true));
            expected.Add(new scannerResponse("CodeGeneratorTests", true));
            expected.Add(new scannerResponse("\"", false));
            expected.Add(new scannerResponse(")", false));
            expected.Add(new scannerResponse(";", false));




            List<scannerResponse> result = scanner.getListOfTokens(given);
            Assert.AreEqual("CodeGeneratorTests", result[7].token);
            CollectionAssert.AreEqual(expected, result);
        }
    }
}