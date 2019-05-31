using Microsoft.VisualStudio.TestTools.UnitTesting;
using System; // one line comment
using System.Collections.Generic;
using System.Text;
/*multiple
 * line
 * comment*/
namespace TestTranslator
{
    [TestClass]
    public class MSTestMix
    {
        [TestInitialize]
        public void MStest1()
        {
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void MStest2()
        {
            Assert.IsTrue(true);
        }
        [TestCleanup]
        public void MStest3()
        {
            Assert.IsTrue(true);
        }
        //[TestMethod]
        //[Combinatorial]
        //public void MStest4()
        //{
            //Assert.IsTrue(true);
        //}
        //[TestMethod]
        //[Pairwise]
        //public void MStest5()
        //{
            //Assert.IsTrue(true);
        //}
        //[TestMethod]
        //[Sequential]
        //public void MStest6()
        //{
            //Assert.IsTrue(true);
        //}
        //[TestMethod]
        //[Theory]
        //public void MStest7()
        //{
            //Assert.IsTrue(true);
        //}
        [ClassInitialize]
        public static void MStest8(TestContext context)
        {
            Assert.IsTrue(true);
        }
        [ClassCleanup]
        public static void MStest9()
        {
            Assert.IsTrue(true);
        }
        //[TestMethod]
        //[Parallelizable]
        //public void MStest10()
        //{
            //Assert.IsTrue(true);
        //}
        //[TestMethod]
        //[RequiresThread]
        //public void MStest11()
        //{
            //Assert.IsTrue(true);
        //}
        //[TestMethod]
        //[Culture]
        //public void MStest12()
        //{
            //Assert.IsTrue(true);
        //}
        //[TestMethod]
        //[Explicit]
        //public void MStest13()
        //{
            //Assert.IsTrue(true);
        //}
        //[TestMethod]
        //[NonParallelizable]
        //public void MStest14()
        //{
            //Assert.IsTrue(true);
        //}
        //[TestMethod]
        //[Repeat(3)]
        //public void MStest15()
        //{
            //Assert.IsTrue(true);
        //}
        //[TestMethod]
        //[Retry(3)]
        //public void MStest16()
        //{
            //Assert.IsTrue(true);
        //}
        //[TestMethod]
        //[TestCaseSource("abc")]
        //public void MStest17(intn)
        //{
            //Assert.IsTrue(true);
            //static int[] abc = { 12, 3, 4 };
        //}
        [TestMethod]
        [Owner("Author")]
        public void MStest18()
        {
            Assert.IsTrue(true);
        }
        [TestMethod]
        [Description("Description")]
        public void MStest19()
        {
            Assert.IsTrue(true);
        }
        [TestMethod]
        [TestCategory("Category")]
        public void MStest20()
        {
            Assert.IsTrue(true);
        }
        [TestMethod]
        [Ignore("Reason")]
        public void MStest21()
        {
            Assert.IsTrue(true);
        }
        //[TestMethod]
        //[DefaultFloatingPointTolerance(2)]
        //public void MStest22()
        //{
            //Assert.IsTrue(true);
        //}
        //[TestMethod]
        //[MaxTime(100)]
        //public void MStest23()
        //{
            //Assert.IsTrue(true);
        //}
        //[TestMethod]
        //[Order(3)]
        //public void MStest24()
        //{
            //Assert.IsTrue(true);
        //}
        //[TestMethod]
        //[SetCulture("fr-FR")]
        //public void MStest25()
        //{
            //Assert.IsTrue(true);
        //}
        //[TestMethod]
        //[SetUICulture("fr-FR")]
        //public void MStest26()
        //{
            //Assert.IsTrue(true);
        //}
        //[TestMethod]
        //[TestOf("typeName")]
        //public void MStest27()
        //{
            //Assert.IsTrue(true);
        //}
    }
    [TestClass]
    public class MSTestClass1
    {
        [TestMethod]
        public void MStest()
        {
            Assert.IsTrue(true);
        }
    }
    //[TestClass]
    //[SetUpFixture]
    //public class MSTestClass2
    //{
        //[TestMethod]
        //public void MStest()
        //{
            //Assert.IsTrue(true);
        //}
    //}
    //[TestClass]
    //[SingleThreaded]
    //public class MSTestClass3
    //{
        //[TestMethod]
        //public void MStest()
        //{
            //Assert.IsTrue(true);
        //}
    //}
    //[TestClass]
    //[TestProperty("aaa", 3)]
    //public class MSTestClass4
    //{
        //[TestMethod]
        //public void MStest()
        //{
            //Assert.IsTrue(true);
        //}
    //}
    //[TestClass]
    //[TestFixtureSource("tfsname")]
    //public class MSTestClass5
    //{
        //[TestMethod]
        //public void MStest()
        //{
            //Assert.IsTrue(true);
        //}
    //}
    //[TestClass]
    //[Parallelizable]
    //public class MSTestClass6
    //{
        //[TestMethod]
        //public void MStest()
        //{
            //Assert.IsTrue(true);
        //}
    //}
    //[TestClass]
    //[RequiresThread]
    //public class MSTestClass7
    //{
        //[TestMethod]
        //public void MStest()
        //{
            //Assert.IsTrue(true);
        //}
    //}
    //[TestClass]
    //[Culture]
    //public class MSTestClass8
    //{
        //[TestMethod]
        //public void MStest()
        //{
            //Assert.IsTrue(true);
        //}
    //}
    //[TestClass]
    //[Explicit]
    //public class MSTestClass9
    //{
        //[TestMethod]
        //public void MStest()
        //{
            //Assert.IsTrue(true);
        //}
    //}
    //[TestClass]
    //[NonParallelizable]
    //public class MSTestClass10
    //{
        //[TestMethod]
        //public void MStest()
        //{
            //Assert.IsTrue(true);
        //}
    //}
    //[TestClass]
    //[Owner("Author")]
    //public class MSTestClass11
    //{
        //[TestMethod]
        //public void MStest()
        //{
            //Assert.IsTrue(true);
        //}
    //}
    //[TestClass]
    //[Description("Description")]
    //public class MSTestClass12
    //{
        //[TestMethod]
        //public void MStest()
        //{
            //Assert.IsTrue(true);
        //}
    //}
    [TestClass]
    [TestCategory("Category")]
    public class MSTestClass13
    {
        [TestMethod]
        public void MStest()
        {
            Assert.IsTrue(true);
        }
    }
    [TestClass]
    [Ignore("Ignore")]
    public class MSTestClass14
    {
        [TestMethod]
        public void MStest()
        {
            Assert.IsTrue(true);
        }
    }
    //[TestClass]
    //[DefaultFloatingPointTolerance(3)]
    //public class MSTestClass15
    //{
        //[TestMethod]
        //public void MStest()
        //{
            //Assert.IsTrue(true);
        //}
    //}
    //[TestClass]
    //[Order(3)]
    //public class MSTestClass16
    //{
        //[TestMethod]
        //public void MStest()
        //{
            //Assert.IsTrue(true);
        //}
    //}
    //[TestClass]
    //[SetCulture("fr-FR")]
    //public class MSTestClass17
    //{
        //[TestMethod]
        //public void MStest()
        //{
            //Assert.IsTrue(true);
        //}
    //}
    //[TestClass]
    //[SetUICulture("fr-FR")]
    //public class MSTestClass18
    //{
        //[TestMethod]
        //public void MStest()
        //{
            //Assert.IsTrue(true);
        //}
    //}
    //[TestClass]
    //[TestOf("typeName")]
    //public class MSTestClass19
    //{
        //[TestMethod]
        //public void MStest()
        //{
            //Assert.IsTrue(true);
        //}
    //}
    [TestClass]
    public class MSAssertionsMix
    {
        [TestMethod]
        public void MStest1()
        {
            Assert.IsTrue(true);
            Assert.IsFalse(false);
        }
        [TestMethod]
        public void MStest2()
        {
            List<int> l = new List<int>();
            CollectionAssert.DoesNotContain(l, 5);
        }
        //[TestMethod]
        //public void MStest3()
        //{
            //StringAssert.DoesNotStartWith("a", " b");
        //}
        //[TestMethod]
        //public void MStest4()
        //{
            //FileAssert.DoesNotExist("path");
        //}
        //[TestMethod]
        //public void MStest5()
        //{
            //DirectoryAssert.Exists("some directory");
        //}
    }
}
