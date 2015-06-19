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
        #region Tests for BytesToHexString

        [TestCase(true)]
        [TestCase(false)]
        public void BytesToHexStringNullInputTest(bool useLowerCase)
        {
            /* testing of null argument
             */
            // arrange

            // act && assert
            Assert.Catch<ArgumentNullException>(() =>
                NumberUtility.BytesToHexString(null, useLowerCase));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void BytesToHexStringZeroLengthInputTest(bool useLowerCase)
        {
            // arrange
            byte[] bytes = new byte[0];

            // act
            string result = NumberUtility.BytesToHexString(bytes, useLowerCase);

            // assert
            Assert.IsTrue(result == string.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void BytesToHexStringTest(bool useLowerCase)
        {
            // arrange
            byte[] bytes = new byte[]
            {
                 0x00 ,0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F,
                 0xFF ,0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70, 0x80, 0x90, 0xA0, 0xB0, 0xC0, 0xD0, 0xE0, 0xF0,
            };
            var sb = new System.Text.StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString(useLowerCase ? "x2" : "X2"));
            }

            // act
            string result = NumberUtility.BytesToHexString(bytes, useLowerCase);

            // assert
            Assert.IsTrue(result == sb.ToString());
        }

        #endregion

        #region Tests for GetRandomInt

        [Test()]
        public void GetRandomIntTest()
        {
            /* testing range [x, y) which x < y
             */
            // arrange
            int minVal = -1;
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

            // act && assert
            Assert.Catch<ArgumentOutOfRangeException>(() =>
                NumberUtility.GetRandomInt(minVal, maxVal));
        }

        #endregion
    }
}
