using HelperLibrary.Core;
using NUnit.Framework;
/* 
 * FileName:    EnumUtilityTests.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  7/16/2015 6:11:03 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Tests
{
    using System;
    using HelperLibrary.Core;
    using NUnit.Framework;

    [TestFixture()]
    public class EnumUtilityTests
    {

        #region Test Data
        public enum TestEnum
        {
            A = 1,
            B = 1,
            C,
        }

        #endregion

        #region Tests For GetEnumTuples

        [Test()]
        public void GetEnumTuplesExceptionTest()
        {
            // act && assert
            Assert.Catch<InvalidOperationException>(
                () => EnumUtility<DateTime>.GetEnumTuples());
        }

        [Test()]
        public void GetEnumTuplesTest()
        {
            // arrange
            // act
            var result = EnumUtility<TestEnum>.GetEnumTuples();

            // assert
            Assert.IsTrue(result.Length == 3);
            foreach (var tuple in result)
            {
                Assert.IsTrue(Enum.GetName(typeof(TestEnum), tuple.Item1) == tuple.Item2);
            }
        }

        #endregion

        #region Tests For GetEnumValue

        [Test]
        public void GetEnumValueNullArgumentTest()
        {
            // act && assert
            Assert.Catch<ArgumentNullException>(
                () => EnumUtility<TestEnum>.GetEnumValue(null));

            // act && assert
            Assert.Catch<ArgumentNullException>(
                () => EnumUtility<TestEnum>.GetEnumValue(""));
        }

        [TestCase("B", TestEnum.B)]
        [TestCase("b", TestEnum.B)]
        public void GetEnumValueTest(string enumName, TestEnum expected)
        {
            // act
            TestEnum result = EnumUtility<TestEnum>.GetEnumValue(enumName);

            // assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetEnumValueInvalidNameTest()
        {
            // arrange
            string enumName = "NoSuchAName";

            // act && assert
            Assert.Catch<InvalidOperationException>(
                () => EnumUtility<TestEnum>.GetEnumValue(enumName));
        }

        #endregion

        #region Tests For GetEnumValue

        [Test()]
        public void GetEnumNameTest()
        {
            // arrange
            TestEnum value = TestEnum.B;

            // act
            string result = EnumUtility<TestEnum>.GetEnumName(value);

            // assert
            Assert.AreEqual("B", result);
        }

        #endregion
    }
}
