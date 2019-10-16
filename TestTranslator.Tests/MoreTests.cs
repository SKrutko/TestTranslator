using NUnit.Framework;
using System.Collections; //comment 1
using System.Collections.Generic;
/*sm;d
 * */
namespace TestTranslator
{
    public static class CoolClass
    {

    }

    [TestFixture]
    class MoreTest
    {

        Parser parser;
        [Test]//comment1
        public void getTokenType_givenUsing_returnsTypeUsing()
        {
            Assert.True(true);//comment 2
        }

        [SetUp]
        public void Setup()
        {
            Assert.False(false);
        }

        [Author("Author")]
        [Test]
        public void test2()
        {
            Assert.IsTrue(true);

            Assert.False(false);

        }
    }

        [SingleThreaded]
        public class MoreTest2
        {
        } //trailing trivia

    [SingleThreaded]
    public class MoreTest3
    {
    } //trailing trivia

}
