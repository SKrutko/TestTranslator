using Microsoft.VisualStudio.TestTools.UnitTesting;using System.Collections; //comment 1
using System.Collections.Generic;
/*sm;d
 * */
namespace TestTranslator
{
[TestClass]    public static class CoolClass
    {

    }

    [TestClass]
public    class MoreTest
    {

        //Parser parser;
        [TestMethod]//comment1
        public void getTokenType_givenUsing_returnsTypeUsing()
        {
            Assert.IsTrue(true);//comment 2
        }
//
//        [SetUp]
//        public void Setup()
//        {
//            Assert.False(false);
//        }
//


        [Owner("Author")]
        [TestMethod]
        public void test2()
        {
            Assert.IsTrue(true);

            Assert.IsFalse(true);

        }
    }
//
//        [SingleThreaded]
//        public class MoreTest2
//        {
        } //trailing trivia
//
//
//    [SingleThreaded]
//    public class MoreTest3
//    {
  //  } //trailing trivia
//

//}

