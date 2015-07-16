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
        private enum TestEnum
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
                () => EnumUtility.GetEnumTuples<DateTime>());
        }

        [Test()]
        public void GetEnumTuplesTest()
        {
            // arrange
            // act
            var result = EnumUtility.GetEnumTuples<TestEnum>();

            // assert
            Assert.IsTrue(result.Length == 3);
            foreach (var tuple in result)
            {
                Assert.IsTrue(Enum.GetName(typeof(TestEnum), tuple.Item1) == tuple.Item2);
            }
        }

        #endregion
    }
}
