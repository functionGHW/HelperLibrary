﻿/* 
 * FileName:    StringUtility.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/26/2015 3:04:31 PM
 * Version:     v1.1
 * Description:
 * */

namespace HelperLibrary.Core
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Helper methods for string.
    /// </summary>
    public static class StringUtility
    {
        /// <summary>
        /// Get the MD5 hash of a string and return as hex string.
        /// </summary>
        /// <param name="theString">the string to compute hash</param>
        /// <param name="useLowerCase">whether to return result as lowercase string</param>
        /// <returns>hash value string</returns>
        /// <exception cref="ArgumentNullException">parameter theString is null.</exception>
        public static string GetMd5OfString(string theString, bool useLowerCase = false)
        {
            if (theString == null)
                throw new ArgumentNullException(nameof(theString));

            byte[] md5Bytes = GetMd5OfStringInternal(theString);
            return NumberUtility.BytesToHexString(md5Bytes, useLowerCase);
        }

        // internal compute hash value of a string
        private static byte[] GetMd5OfStringInternal(string theString)
        {
            Contract.Assert(theString != null);

            byte[] orgBytes = Encoding.UTF8.GetBytes(theString);

            // The default hash algorithm is provided by MD5CryptoServiceProvider 
            using (MD5 md5 = MD5.Create())
            {
                byte[] md5Bytes = md5.ComputeHash(orgBytes);
                return md5Bytes;
            }
        }
    }
}
