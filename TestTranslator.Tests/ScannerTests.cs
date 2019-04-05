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
            List<string> expected = new List<string>();
            List<string> result = scanner.getListOfTokens(given);
            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void getListOfTokens_givenOneWord_returnListWithOneElement()
        {
            string given = "using";
            List<string> expected = new List<string>();
            expected.Add("using");

            List<string> result = scanner.getListOfTokens(given);
            Assert.IsTrue(true);
            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void getListOfTokens_givenTwoWords_returnListWithTwoElements()
        {
            string given = "using System";
            List<string> expected = new List<string>();
            expected.Add("using");
            expected.Add("System");

            List<string> result = scanner.getListOfTokens(given);

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void getListOfTokens_givenTwoWordsWithSpaces_returnListWithTwoElements()
        {
            string given = "using   System   ";
            List<string> expected = new List<string>();
            expected.Add("using");
            expected.Add("System");

            List<string> result = scanner.getListOfTokens(given);

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void getListOfTokens_givenWordsWithSpecialChar_returnList()
        {
            string given = "using System.Framework";
            List<string> expected = new List<string>();
            expected.Add("using");
            expected.Add("System");
            expected.Add(".");
            expected.Add("Framework");

            List<string> result = scanner.getListOfTokens(given);

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void getListOfTokens_givenWordsWithSpecialChar_returnList2()
        {
            string given = "using System.Framework;";
            List<string> expected = new List<string>();
            expected.Add("using");
            expected.Add("System");
            expected.Add(".");
            expected.Add("Framework");
            expected.Add(";");
            List<string> result = scanner.getListOfTokens(given);

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void getListOfTokens_givenComment_returnList()
        {
            string given = "//";
            List<string> expected = new List<string>();
            expected.Add("//");

            List<string> result = scanner.getListOfTokens(given);

            CollectionAssert.AreEqual(expected, result);

        }

        [Test]
        public void getListOfTokens_givenSpecialChar_returnList()
        {
            string given = "// aaa;a /* aaaaaa*/ aaaa";
            List<string> expected = new List<string>();
            expected.Add("//");
            expected.Add("aaa");
            expected.Add(";");
            expected.Add("a");
            expected.Add("/*");
            expected.Add("aaaaaa");
            expected.Add("*/");
            expected.Add("aaaa");

            List<string> result = scanner.getListOfTokens(given);

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
            List<string> expected = new List<string>();
            expected.Add("/*");
            expected.Add("//");
            expected.Add("aaa");
            expected.Add(";");
            expected.Add("a");
            expected.Add("/*");
            expected.Add("aaaaaa");
            expected.Add("*/");
            expected.Add(";");
            expected.Add("aaaa");

            List<string> result = scanner.getListOfTokens(given);

            CollectionAssert.AreEqual(expected, result);
        }
    }
}