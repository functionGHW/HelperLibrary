/* 
 * FileName:    Security.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  2017/3/3 13:45:11
 * Version:     v1.0
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core
{
    public static class Security
    {
        /// <summary>
        /// Encrypt password using MD5 with a salt string.
        /// Algorithm: 
        ///     md5 hash -> concat with salt -> md5 hash again
        /// </summary>
        /// <param name="password">password string</param>
        /// <param name="salt">salt string</param>
        /// <returns>md5 hash string</returns>
        public static string Md5EncryptWithSalt(string password, string salt)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            if (salt == null)
                throw new ArgumentNullException(nameof(salt));
            
            string firstHash = Md5Encrypt(password);
            string secondHash = Md5Encrypt(firstHash + salt);
            return secondHash;
        }

        /// <summary>
        /// Get the MD5 hash of a string and return as hex string.
        /// </summary>
        /// <param name="theString">the string to compute hash</param>
        /// <param name="useLowerCase">whether to return result as lowercase string</param>
        /// <returns>hash value string</returns>
        /// <exception cref="ArgumentNullException">parameter theString is null.</exception>
        public static string Md5Encrypt(string theString, bool useLowerCase = true)
        {
            if (theString == null)
                throw new ArgumentNullException(nameof(theString));

            byte[] md5Bytes = GetMd5Internal(theString);
            return NumberUtility.BytesToHexString(md5Bytes, useLowerCase);
        }

        // internal compute hash value of a string
        private static byte[] GetMd5Internal(string theString)
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
