/* 
 * FileName:    StringExtensionsTests.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  6/23/2015 9:08:49 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.ExtensionHelper.Tests
{
    using HelperLibrary.Core.ExtensionHelper;
    using NUnit.Framework;
    using System;

    [TestFixture()]
    public class StringExtensionsTests
    {
        #region Tests For ReverseString

        [Test]
        public void ReverseStringNullInputTest()
        {
            // arrange
            string str = null;

            // act && assert
            Assert.Catch<ArgumentNullException>(
                () => StringExtensions.ReverseString(str));
        }

        [TestCase("", "")]
        [TestCase("a", "a")]
        [TestCase("aB", "Ba")]
        [TestCase("Abc", "cbA")]
        public void ReverseStringTest(string orgStr, string expected)
        {
            // arrange

            // act
            string result = StringExtensions.ReverseString(orgStr);

            // accert
            Assert.AreEqual(expected, result);
        }

        #endregion
    }
}
