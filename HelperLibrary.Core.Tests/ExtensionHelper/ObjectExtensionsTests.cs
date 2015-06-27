/* 
 * FileName:    ObjectExtensionsTests.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  6/23/2015 9:23:43 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.ExtensionHelper.Tests
{
    using HelperLibrary.Core.ExtensionHelper;
    using NUnit.Framework;
    using System;

    [TestFixture()]
    public class ObjectExtensionsTests
    {
        #region Tests For ReverseArray

        [Test]
        public void ReverseArrayNullInputTest()
        {
            // arrange
            int[] ary = null;

            // act && assert
            Assert.Catch<ArgumentNullException>(
                () => ObjectExtensions.ReverseArray(ary));

            Assert.Catch<ArgumentNullException>(
               () => ObjectExtensions.ReverseArray(ary, 0, 0));
        }

        [TestCase("", -1, 0)] // startIndex out of range
        [TestCase("", 0, -1)] // length out of range
        [TestCase("", 1, 0)]  // startIndex plus length out of range
        public void ReverseArrayIndexOutOfRangeTest(string orgStr, int startIndex, int length)
        {
            // arrange
            char[] ary = orgStr.ToCharArray();

            // act && assert
            Assert.Catch<IndexOutOfRangeException>(
                () => ObjectExtensions.ReverseArray(ary, startIndex, length));
        }

        [TestCase("", "")]
        [TestCase("A", "A")]
        [TestCase("aB", "Ba")]
        [TestCase("aBc", "cBa")]
        public void ReverseArrayTest(string orgStr, string expected)
        {
            // arrange
            char[] ary = orgStr.ToCharArray();

            // act
            ObjectExtensions.ReverseArray(ary);

            // assert
            for (int i = 0; i < ary.Length; i++)
            {
                Assert.AreEqual(expected[i], ary[i]);
            }
        }

        [TestCase("ABCD", 0, 0, "ABCD")]
        [TestCase("ABCD", 0, 1, "ABCD")]
        [TestCase("ABCD", 0, 2, "BACD")]
        [TestCase("ABCD", 0, 3, "CBAD")]
        public void ReverseArrayWithIndexTest(string orgStr, int startIndex, int length, string expected)
        {
            // arrange
            char[] ary = orgStr.ToCharArray();

            // act
            ObjectExtensions.ReverseArray(ary, startIndex, length);

            // assert
            for (int i = 0; i < ary.Length; i++)
            {
                Assert.AreEqual(expected[i], ary[i]);
            }
        }

        #endregion
    }
}
