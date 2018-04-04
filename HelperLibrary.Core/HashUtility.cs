/* 
 * FileName:    HashUtility.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  4/4/2018 10:39:19 AM
 * Version:     v1.0
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core
{
    /// <summary>
    /// provide some method for handling hash
    /// </summary>
    public class HashUtility
    {
        /// <summary>
        /// Get the MD5 hash of a string and return as hex string.
        /// </summary>
        /// <param name="theString">the string to compute hash</param>
        /// <param name="useLowerCase">whether to return result as lowercase string</param>
        /// <returns>hash value string</returns>
        /// <exception cref="ArgumentNullException">parameter theString is null.</exception>
        public static string GetStringMd5(string theString, bool useLowerCase = true)
        {
            if (theString == null)
                throw new ArgumentNullException(nameof(theString));

            byte[] orgBytes = Encoding.UTF8.GetBytes(theString);

            // The default hash algorithm is provided by MD5CryptoServiceProvider 
            using (MD5 md5 = MD5.Create())
            {
                byte[] md5Bytes = md5.ComputeHash(orgBytes);
                return NumberUtility.BytesToHexString(md5Bytes, useLowerCase);
            }
        }

        /// <summary>
        /// Get md5 hash of file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="useLowerCase"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetFileMd5(string filePath, bool useLowerCase = true)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            using (var fs = File.OpenRead(filePath))
            {
                return GetStreamMd5(fs, useLowerCase);
            }
        }

        /// <summary>
        /// get md5 hash of a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="useLowerCase"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetStreamMd5(Stream stream, bool useLowerCase = true)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            using (MD5 md5 = MD5.Create())
            {
                byte[] md5Bytes = md5.ComputeHash(stream);
                return NumberUtility.BytesToHexString(md5Bytes, useLowerCase);
            }
        }
        
    }
}
