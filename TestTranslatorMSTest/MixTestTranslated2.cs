using System; // one line comment
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;/*multiple
 * line
 * comment*/

namespace TestTranslator
{
[TestClass]
    public class TestMix222
    {
//        [SetUp]
//        public void test1()
//        {
//            Assert.True(true);
//        }
//

        [TestMethod]
        public void test2()
        {
            Assert.IsTrue(true);
        }
//        [TearDown]
//        public void test3()
//        {
//            Assert.True(true);
//        }
//
//
//        [Test]
//        [Combinatorial]
//        public void test4()
//        {
//            Assert.True(true);
//        }
//
//
//        [Test]
//        [Pairwise]
//        public void test5()
//        {
//            Assert.True(true);
//        }
//
//
//        [Test]
//        [Sequential]
//        public void test6()
//        {
//            Assert.True(true);
//        }
//
//
//        [Test]
//        [Theory]
//        public void test7()
//        {
//            Assert.True(true);
//        }
//
//
//
//        [OneTimeSetUp]
//        public void test8()
//        {
//            Assert.True(true);
//        }
//
//
//        [OneTimeTearDown]
//        public void test9()
//        {
//            Assert.True(true);
//        }
//
//
//
//        [Test]
//        [Parallelizable]
//        public void test10()
//        {
//            Assert.True(true);
//        }
//
//
//        [Test]
//        [RequiresThread]
//        public void test11()
//        {
//            Assert.True(true);
//        }
//
//
//        [Test]
//        [Culture]
//        public void test12()
//        {
//            Assert.True(true);
//        }
//
//
//        [Test]
//        [Explicit]
//        public void test13()
//        {
//            Assert.True(true);
//        }
//
//
//        [Test]
//        [NonParallelizable]
//        public void test14()
//        {
//            Assert.True(true);
//        }
//
//
//        [Test]
//        [Repeat(3)]
//        public void test15()
//        {
//            Assert.True(true);
//        }
//
//
//        [Test]
//        [Retry(3)]
//        public void test16()
//        {
//            Assert.True(true);
//        }
//
//
//        [Test]
//        [TestCaseSource("abc")]
//        public void test17(int n)
//        {
//            Assert.True(true);
//        }
//

        static int[] abc = { 12, 3, 4 };
        [TestMethod]
        [Owner("Author")]
        public void test18()
        {
            Assert.IsTrue(true);
        }
        [TestMethod]
        [Description("Description")]
        public void test19()
        {
            Assert.IsTrue(true);
        }
        [TestMethod]
        [TestCategory("Category")]
        public void test20()
        {
            Assert.IsTrue(true);
        }
        [TestMethod]
        [Ignore("Reason")]
        public void test21()
        {
            Assert.IsTrue(true);
        }
//        [Test]
//        [DefaultFloatingPointTolerance(2)]
//        public void test22()
//        {
//            Assert.True(true);
//        }
//
//
//        [Test]
//        [MaxTime(100)]
//        public void test23()
//        {
//            Assert.True(true);
//        }
//
//
//        [Test]
//        [Order(3)]
//        public void test24()
//        {
//            Assert.True(true);
//        }
//
//
//        [Test]
//        [SetCulture("fr-FR")]
//        public void test25()
//        {
//            Assert.True(true);
//        }
//
//
//        [Test]
//        [SetUICulture("fr-FR")]
//        public void test26()
//        {
//            Assert.True(true);
//        }
//
//
//        [Test]
//        [TestOf("typeName")]
//        public void test27()
//        {
//            Assert.True(true);
//        }
//

        
    }
   
    [TestClass]
    public class TestClass1
    {
        [TestMethod]
        public void test()
        {
            Assert.IsTrue(true);
        }
    }
//    [SetUpFixture]
//    class TestClass2
//    {
//        [Test]
//        public void test()
//        {
//            Assert.True(true);
//        }
//    }
//
//    [SingleThreaded]
//    class TestClass3
//    {
//        [Test]
//        public void test()
//        {
//            Assert.True(true);
//        }
//    }
//
//    [Property("aaa", 3)]
//    class TestClass4
//    {
//        [Test]
//        public void test()
//        {
//            Assert.True(true);
//        }
//    }
//
//    [TestFixtureSource("tfsname")]
//    class TestClass5
//    {
//        [Test]
//        public void test()
//        {
//            Assert.True(true);
//        }
//    }
//
//   
//    [Parallelizable]
//    class TestClass6
//    {
//        [Test]
//        public void test()
//        {
//            Assert.True(true);
//        }
//    }
//
//    [RequiresThread]
//    class TestClass7
//    {
//        [Test]
//        public void test()
//        {
//            Assert.True(true);
//        }
//    }
//
//    [Culture]
//    class TestClass8
//    {
//        [Test]
//        public void test()
//        {
//            Assert.True(true);
//        }
//    }
//
//    [Explicit]
//    class TestClass9
//    {
//        [Test]
//        public void test()
//        {
//            Assert.True(true);
//        }
//    }
//
//    [NonParallelizable]
//    class TestClass10
//    {
//        [Test]
//        public void test()
//        {
//            Assert.True(true);
//        }
//    }
//
//   
//    [Author("Author")]
//    class TestClass11
//    {
//        [Test]
//        public void test()
//        {
//            Assert.True(true);
//        }
//    }
//
//    [Description("Description")]
//    class TestClass12
//    {
//        [Test]
//        public void test()
//        {
//            Assert.True(true);
//        }
//    }
//
    [TestCategory("Category")]
[TestClass]
    public class TestClass13
    {
        [TestMethod]
        public void test()
        {
            Assert.IsTrue(true);
        }
    }
    [Ignore("Ignore")]
[TestClass]
    public class TestClass14
    {
        [TestMethod]
        public void test()
        {
            Assert.IsTrue(true);
        }
    }
//[TestClass]//    [DefaultFloatingPointTolerance(3)]
//    class TestClass15
//    {
//        [Test]
//        public void test()
//        {
//            Assert.True(true);
//        }
//    }
//
//    [Order(3)]
//    class TestClass16
//    {
//        [Test]
//        public void test()
//        {
//            Assert.True(true);
//        }
//    }
//
//    [SetCulture("fr-FR")]
//    class TestClass17
//    {
//        [Test]
//        public void test()
//        {
//            Assert.True(true);
//        }
//    }
//
//    [SetUICulture("fr-FR")]
//    class TestClass18
//    {
//        [Test]
//        public void test()
//        {
//            Assert.True(true);
//        }
//    }
//
//    [TestOf("typeName")]
//    class TestClass19
//    {
//        [Test]
//        public void test()
//        {
//            Assert.True(true);
//        }
//    }
//

        [TestClass]
    public class AssertionsMix
    {
        [TestMethod]
        public void test1()
        {
            Assert.IsTrue(true);
            Assert.IsFalse(false);
        }
        [TestMethod]
        public void test2()
        {
            List<int> l = new List<int>();
//            CollectionAssert.DoesNotContain(l, 5);
        }
        [TestMethod]
        public void test3()
        {
//            StringAssert.DoesNotStartWith("a", "b");
        }
        [TestMethod]
        public void test4()
        {
//            FileAssert.DoesNotExist("path");
        }
        [TestMethod]
        public void test5()
        {
//            DirectoryAssert.Exists("some directory");
        }
    }


}

