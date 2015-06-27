/* 
 * FileName:    StringUtilityTests.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  5/14/2015 6:07:02 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Tests
{
    using HelperLibrary.Core;
    using NUnit.Framework;
    using System;

    [TestFixture()]
    public class StringUtilityTests
    {
        #region Test Datas

        private object[] md5TestData = 
        {
            new object[] { "", true, "d41d8cd98f00b204e9800998ecf8427e" },
            new object[] { "a", true, "0cc175b9c0f1b6a831c399e269772661" },
            new object[] { "abc", true, "900150983cd24fb0d6963f7d28e17f72" },
            new object[] { "message digest", true, "f96b697d7cb7938d525a2f31aaf161d0" },
            new object[] { "abcdefghijklmnopqrstuvwxyz", true, "c3fcd3d76192e4007dfb496cca67e13b" },
            new object[] { "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", true, "d174ab98d277d9f5a5611c2c9f419d9f" },
            new object[] { "12345678901234567890123456789012345678901234567890123456789012345678901234567890", true, "57edf4a22be3c955ac49da2e2107b67a" },

            new object[] { "", false, "D41D8CD98F00B204E9800998ECF8427E" },
            new object[] { "a", false, "0CC175B9C0F1B6A831C399E269772661" },
            new object[] { "abc", false, "900150983CD24FB0D6963F7D28E17F72" },
            new object[] { "message digest", false, "F96B697D7CB7938D525A2F31AAF161D0" },
            new object[] { "abcdefghijklmnopqrstuvwxyz", false, "C3FCD3D76192E4007DFB496CCA67E13B" },
            new object[] { "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", false, "D174AB98D277D9F5A5611C2C9F419D9F" },
            new object[] { "12345678901234567890123456789012345678901234567890123456789012345678901234567890", false, "57EDF4A22BE3C955AC49DA2E2107B67A" },
        };

        #endregion

        #region Tests For GetMD5OfString

        [Test()]
        public void GetMD5OfStringNullInputTest()
        {
            /* testing null argument
             */
            // arrange

            // act && assert
            Assert.Catch<ArgumentNullException>(() =>
                StringUtility.GetMD5OfString(null, true));
        }

        [TestCaseSource("md5TestData")]
        public void GetMD5OfStringTest(string orgStr, bool userLowerCase, string md5Str)
        {
            /* testing GetMD5OfString,
             * for each orgStr as input, the result should equal to the md5Str
             */
            // arrange
            string theString = orgStr;

            // act
            string lowerCaseResult = StringUtility.GetMD5OfString(theString, userLowerCase);

            // assert
            Assert.IsTrue(lowerCaseResult == md5Str);
        }

        #endregion
    }
}
