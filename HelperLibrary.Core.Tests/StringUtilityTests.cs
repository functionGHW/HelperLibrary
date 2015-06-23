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
            new string[] { "", "d41d8cd98f00b204e9800998ecf8427e" },
            new string[] { "a", "0cc175b9c0f1b6a831c399e269772661" },
            new string[] { "abc", "900150983cd24fb0d6963f7d28e17f72" },
            new string[] { "message digest", "f96b697d7cb7938d525a2f31aaf161d0" },
            new string[] { "abcdefghijklmnopqrstuvwxyz", "c3fcd3d76192e4007dfb496cca67e13b" },
            new string[] { "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", "d174ab98d277d9f5a5611c2c9f419d9f" },
            new string[] { "12345678901234567890123456789012345678901234567890123456789012345678901234567890", "57edf4a22be3c955ac49da2e2107b67a" },
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

            Assert.Catch<ArgumentNullException>(() =>
                StringUtility.GetMD5OfString(null, false));
        }

        [TestCaseSource("md5TestData")]
        public void GetMD5OfStringTest(string orgStr, string md5Str)
        {
            /* testing GetMD5OfString,
             * for each orgStr as input, the result should equal to the md5Str
             */
            // arrange
            string theString = orgStr;

            // act
            string lowerCaseResult = StringUtility.GetMD5OfString(theString, true);
            string upperCaseResult = StringUtility.GetMD5OfString(theString, false);

            // assert
            Assert.IsTrue(lowerCaseResult == md5Str);
            Assert.IsTrue(upperCaseResult == md5Str.ToUpperInvariant());
        }

        #endregion
    }
}
