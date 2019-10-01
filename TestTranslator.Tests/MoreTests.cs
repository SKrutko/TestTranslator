using NUnit.Framework;
using System.Collections; //comment 1
using System.Collections.Generic;
/*sm;d
 * */
namespace TestTranslator
{
    public class MoreTest
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

        }


        [Author("Author")]
        [Test]
        public void test2()
        {
            Assert.True(true);
        }
    }
}
