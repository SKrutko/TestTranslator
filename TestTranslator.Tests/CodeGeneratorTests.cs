using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;


namespace TestTranslator
{
    class CodeGeneratorTests
    {
        CodeGenerator codeGenerator;
        [SetUp]
        public void Setup()
        {
            codeGenerator = new CodeGenerator();
        }

        [Test]
        public void generate_givenEmptyDoc_returnOneUsing()
        {
            Document given = new Document();
            List<string> expected = new List<string>();
            expected.Add("using Microsoft.VisualStudio.TestTools.UnitTesting;");
            CollectionAssert.AreEqual(expected, codeGenerator.TranslateDocument(given));
        }

        [Test]
        public void generate_givenUsing_return2Using()
        {
            Document given = new Document();
            given.addUsingStatement("System");

            List<string> expected = new List<string>();
            expected.Add("using Microsoft.VisualStudio.TestTools.UnitTesting;");
            expected.Add("using System;");

            CollectionAssert.AreEqual(expected, codeGenerator.TranslateDocument(given));
        }

        [Test]
        public void generate_given2Using_return3Using()
        {
            Document given = new Document();
            given.addUsingStatement("System");
            given.addUsingStatement("System.Collections");

            List<string> expected = new List<string>();
            expected.Add("using Microsoft.VisualStudio.TestTools.UnitTesting;");
            expected.Add("using System;");
            expected.Add("using System.Collections;");

            CollectionAssert.AreEqual(expected, codeGenerator.TranslateDocument(given));
        }

        [Test]
        public void generate_givenNamespace_returnUsingNamespace()
        {
            Document given = new Document();
            given.addNamespaceStatement("TestTranslator.Test");

            List<string> expected = new List<string>();
            expected.Add("using Microsoft.VisualStudio.TestTools.UnitTesting;");
            expected.Add("namespace TestTranslator.Test");
            expected.Add("{");
            expected.Add("}");

            CollectionAssert.AreEqual(expected, codeGenerator.TranslateDocument(given));
        }
        [Test]
        public void generate_givenClass_returnUsingClass()
        {
            List<Attribute> givenClassAttributes = new List<Attribute>();
            givenClassAttributes.Add(new Attribute(AttributeType.ClassAttribute, "TestFixture"));
            Document given = new Document();
            given.addToStructure(documentUnit.ClassAttributeWithoutArgs);
            given.addClass("CodeGeneratorTests", givenClassAttributes);

            List<string> expected = new List<string>();
            expected.Add("using Microsoft.VisualStudio.TestTools.UnitTesting;");
            expected.Add("    [TestClass]");
            expected.Add("    public class CodeGeneratorTests");
            expected.Add("    {");
            expected.Add("    }");

            CollectionAssert.AreEqual(expected, codeGenerator.TranslateDocument(given));
        }

        [Test]
        public void generate_given2Classes_returnUsing2Classe()
        {
            List<Attribute> givenClassAttributes1 = new List<Attribute>();
            givenClassAttributes1.Add(new Attribute(AttributeType.ClassAttribute, "TestFixture"));
            List<Attribute> givenClassAttributes2 = new List<Attribute>();
            givenClassAttributes2.Add(new Attribute(AttributeType.ClassAttribute, "SingleThreaded"));
            Document given = new Document();
            given.addToStructure(documentUnit.ClassAttributeWithoutArgs);
            given.addClass("CodeGeneratorTests", givenClassAttributes1);
            given.addToStructure(documentUnit.ClassAttributeWithoutArgs);
            given.addComment("one line comment", true);
            given.addClass("CodeGeneratorTests2", givenClassAttributes2);

            List<string> expected = new List<string>();
            expected.Add("using Microsoft.VisualStudio.TestTools.UnitTesting;");
            expected.Add("    [TestClass]");
            expected.Add("    public class CodeGeneratorTests");
            expected.Add("    {");
            expected.Add("    }");
            expected.Add("    [SingleThreaded] //one line comment");
            expected.Add("    public class CodeGeneratorTests2");
            expected.Add("    {");
            expected.Add("    }");

            CollectionAssert.AreEqual(expected, codeGenerator.TranslateDocument(given));
        }

        [Test]
        public void generate_givenClassWithCodeLines()
        {
            List<Attribute> givenClassAttributes = new List<Attribute>();
            givenClassAttributes.Add(new Attribute(AttributeType.ClassAttribute, "TestFixture"));
            
            Document given = new Document();
            given.addToStructure(documentUnit.ClassAttributeWithoutArgs);
            given.addClass("CodeGeneratorTests", givenClassAttributes);

            given.addCodeLine("int i = 0;");
            given.addCodeLine("i++;");

            List<string> expected = new List<string>();
            expected.Add("using Microsoft.VisualStudio.TestTools.UnitTesting;");
            expected.Add("    [TestClass]");
            expected.Add("    public class CodeGeneratorTests");
            expected.Add("    {");
            expected.Add("        int i = 0;");
            expected.Add("        i++;");
            expected.Add("    }");
            
            CollectionAssert.AreEqual(expected, codeGenerator.TranslateDocument(given));
        }

        [Test]
        public void generate_givenClassWithTestMethod()
        {
            List<Attribute> givenClassAttributes = new List<Attribute>();
            givenClassAttributes.Add(new Attribute(AttributeType.ClassAttribute, "TestFixture"));

            Document given = new Document();
            given.addToStructure(documentUnit.ClassAttributeWithoutArgs);
            given.addClass("CodeGeneratorTests", givenClassAttributes);

            given.addToStructure(documentUnit.TestAttributeWithoutArgs);
            List<Attribute> givenListOfAttr = new List<Attribute>();
            givenListOfAttr.Add(new Attribute(AttributeType.TestAttribute, "SetUp"));

            TestMethod setUp = new TestMethod("void", givenListOfAttr);
            setUp.AddArgs("");
            setUp.SetName("SetupMethod");
            given.AddTestMethod(setUp);

            given.addToStructure(documentUnit.TestAttributeWithoutArgs);
            List<Attribute> givenListOfAttr1 = new List<Attribute>();
            givenListOfAttr1.Add(new Attribute(AttributeType.TestAttribute, "Test"));

            TestMethod test = new TestMethod("void", givenListOfAttr1);
            test.AddArgs("");
            test.SetName("TestMethod1");
            given.AddTestMethod(test);

            List<string> expected = new List<string>();
            expected.Add("using Microsoft.VisualStudio.TestTools.UnitTesting;");
            expected.Add("    [TestClass]");
            expected.Add("    public class CodeGeneratorTests");
            expected.Add("    {");
            expected.Add("        [TestInitialize]");
            expected.Add("        public void SetupMethod()");
            expected.Add("        {");
            expected.Add("        }");
            expected.Add("        [TestCase]");
            expected.Add("        public void TestMethod1()");
            expected.Add("        {");
            expected.Add("        }");
            expected.Add("    }");

            CollectionAssert.AreEqual(expected, codeGenerator.TranslateDocument(given));
        }

        [Test]
        public void generate_givenClassWithTestMethodWithAssertions()
        {
            List<Attribute> givenClassAttributes = new List<Attribute>();
            givenClassAttributes.Add(new Attribute(AttributeType.ClassAttribute, "TestFixture"));

            Document given = new Document();
            given.addToStructure(documentUnit.ClassAttributeWithoutArgs);
            given.addClass("CodeGeneratorTests", givenClassAttributes);

            given.addToStructure(documentUnit.TestAttributeWithoutArgs);
            List<Attribute> givenListOfAttr1 = new List<Attribute>();
            givenListOfAttr1.Add(new Attribute(AttributeType.TestAttribute, "Test"));

            TestMethod test = new TestMethod("void", givenListOfAttr1);
            test.AddArgs("");
            test.SetName("TestMethod1");
            given.AddTestMethod(test);

            Assertion a1 = new Assertion("Assert");
            a1.SetMethod("AreEqual");
            a1.SetArgs("1, 1");
            given.AddAssertion(a1);

            Assertion a2 = new Assertion("CollectionAssert");
            a2.SetMethod("AreEqual");
            a2.SetArgs("b, c");
            given.AddAssertion(a2);



            List<string> expected = new List<string>();
            expected.Add("using Microsoft.VisualStudio.TestTools.UnitTesting;");
            expected.Add("    [TestClass]");
            expected.Add("    public class CodeGeneratorTests");
            expected.Add("    {");
            expected.Add("        [TestCase]");
            expected.Add("        public void TestMethod1()");
            expected.Add("        {");
            expected.Add("            Assert.AreEqual(1, 1);");
            expected.Add("            CollectionAssert.AreEqual(b, c);");
            expected.Add("        }");
            expected.Add("    }");

            CollectionAssert.AreEqual(expected, codeGenerator.TranslateDocument(given));
        }

        [Test]
        public void generateTest()
        {
            Document given = new Document();
            given.addUsingStatement("System");
            given.addUsingStatement("System.Collections");
            given.addComment("one line comment", true);
            given.addNamespaceStatement("TestTranslator.Test");
           

            List<string> expected = new List<string>();
            expected.Add("using Microsoft.VisualStudio.TestTools.UnitTesting;");
            expected.Add("using System;");
            expected.Add("using System.Collections; //one line comment");
            expected.Add("namespace TestTranslator.Test");
            expected.Add("{");
            expected.Add("}");

            CollectionAssert.AreEqual(expected, codeGenerator.TranslateDocument(given));
        }
    }
}
