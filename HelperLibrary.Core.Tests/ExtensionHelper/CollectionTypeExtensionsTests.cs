using HelperLibrary.Core.ExtensionHelper;
using NUnit.Framework;
/* 
 * FileName:    CollectionTypeExtensionsTests.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  8/19/2015 3:48:04 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.ExtensionHelper.Tests
{
    using HelperLibrary.Core.ExtensionHelper;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture()]
    public class CollectionTypeExtensionsTests
    {
        [Test()]
        public void IsNullOrEmptyTest()
        {
            // arrange 
            List<int> list = new List<int>() { 1 };
            List<int> emptyList = new List<int>();
            List<int> nullList = null;

            // act && assert
            Assert.IsFalse(list.IsNullOrEmpty(), "list should have elements");
            Assert.IsTrue(emptyList.IsNullOrEmpty(), "list should be empty");
            Assert.IsTrue(nullList.IsNullOrEmpty(), "list should be null");
        }

        [Test()]
        public void AddRangeTest()
        {
            // arrange
            var list = new List<int> { };
            var listToAdd = new List<int> { 1, 2, 3 };

            // act
            list.AddRange(listToAdd);

            // assert
            Assert.IsTrue(list.Contains(1));
            Assert.IsTrue(list.Contains(2));
            Assert.IsTrue(list.Contains(3));

            Assert.AreEqual(3, list.Count);
        }

        [Test()]
        public void AddRangeTransformTest()
        {
            // arrange
            var list = new List<string> { };
            var listToAdd = new List<int> { 1, 2, 3 };

            // act
            list.AddRange(listToAdd, i => i.ToString());

            // assert
            Assert.IsTrue(list.Contains("1"));
            Assert.IsTrue(list.Contains("2"));
            Assert.IsTrue(list.Contains("3"));

            Assert.AreEqual(3, list.Count);
        }
    }
}
