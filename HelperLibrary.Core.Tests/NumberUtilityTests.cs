/* 
 * FileName:    NumberUtilityTests.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  5/14/2015 2:40:36 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Tests
{
    using HelperLibrary.Core;
    using NUnit.Framework;
    using System;

    [TestFixture()]
    public class NumberUtilityTests
    {
        [Test()]
        public void BytesToHexStringNullInputTest()
        {
            /* testing of null argument
             */
            // arrange

            // act && assert
            Assert.Catch<ArgumentNullException>(() =>
                NumberUtility.BytesToHexString(null, false));
        }

        [Test()]
        public void BytesToHexStringTest()
        {
            // arrange
            byte[] bytes = new byte[0];
            bool useLowerCase = false;

            // act
            string result = NumberUtility.BytesToHexString(bytes, useLowerCase);

            // assert
            Assert.IsTrue(result == string.Empty);
        }

        [Test()]
        public void GetRandomIntTest()
        {
            /* testing range [x, y) which x < y
             */
            // arrange
            int minVal = 2;
            int maxVal = 5;

            int result = NumberUtility.GetRandomInt(minVal, maxVal);

            // assert
            Assert.IsTrue(result >= minVal && result < maxVal);
        }

        [TestCase(-2, -1)] // [x, x + 1) => result can only be the minVal
        [TestCase(1, 1)] // [x, x) => result must be same as minVal
        public void GetRandomIntBoundaryValueTest(int minVal, int maxVal)
        {
            /* boundary values testing of range [x, x + 1) 
             * and range [x, x)
             */
            // arrange
            
            // act
            int result = NumberUtility.GetRandomInt(minVal, maxVal);

            // assert
            Assert.IsTrue(result == minVal);
        }

        [Test()]
        public void GetRandomIntOutOfRangeTest()
        {
            /* exception testing of range [x, y), which x > y
             */
            // arrange
            int minVal = 123;
            int maxVal = 10;

            // Assert.IsTrue(minVal > maxVal);

            // act && assert
            Assert.Catch<ArgumentOutOfRangeException>(() =>
                NumberUtility.GetRandomInt(minVal, maxVal));
        }
    }
}
