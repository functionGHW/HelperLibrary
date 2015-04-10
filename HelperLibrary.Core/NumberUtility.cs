/* 
 * FileName:    NumberUtility.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/11/2015 10:26:19 AM
 * Version:     v1.2
 * Description:
 * */

namespace HelperLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class NumberUtility
    {
        #region Fields

        private static Lazy<Random> randLazy = new Lazy<Random>();

        // use a static char set to make it a bit more fast.
        private static readonly char[][] hexCharSet = 
        { 
            // index 0 is upper case chars
            new []{'0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F'},
            // index 1 is lower case chars
            new []{'0','1','2','3','4','5','6','7','8','9','a','b','c','d','e','f'},
        };

        #endregion


        #region Properties

        /* define some ussful constants for store capacity and file size.
         */
        /// <summary>
        /// 1 Kilo Bytes
        /// </summary>
        public const long OneKB = 1024;

        /// <summary>
        /// one Mega Bytes
        /// </summary>
        public const long OneMB = 1024 * OneKB;

        /// <summary>
        /// one Giga Bytes
        /// </summary>
        public const long OneGB = 1024 * OneMB;

        /// <summary>
        /// one Tera Bytes
        /// </summary>
        public const long OneTB = 1024 * OneGB;

        /// <summary>
        /// one Peta Bytes
        /// </summary>
        public const long OnePB = 1024 * OneTB;

        /// <summary>
        /// one Exa Bytes
        /// </summary>
        public const long OneEB = 1024 * OnePB;

        #endregion

        #region Methods

        /// <summary>
        /// Convert byte array to hex format string
        /// </summary>
        /// <param name="bytes">the array</param>
        /// <param name="useLowerCase"></param>
        /// <returns>return the string of all byte with hex format
        /// if no byte to convert, return empty string</returns>
        /// <exception cref="ArgumentNullException">bytes is null</exception>
        public static string BytesToHexString(byte[] bytes, bool useLowerCase = false)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            int len = bytes.Length;
            if (len == 0)
            {
                return String.Empty;
            }

            Contract.Assert(len > 0);
            char[] charAry = new char[len * 2];

            // get hex char set to use.
            char[] charSet = hexCharSet[useLowerCase ? 1 : 0];
            for (int i = 0; i < len; i++)
            {
                int b = bytes[i];
                int charPos = i * 2;

                // get higher 4 bits of byte
                int hexValue = (b & 0x000000F0) >> 4;
                charAry[charPos] = charSet[hexValue];

                // get lower 4 bits of byte
                hexValue = b & 0x0000000F;
                charAry[charPos + 1] = charSet[hexValue];
            }
            return new String(charAry);
        }

        /// <summary>
        /// a static method to get a random Int32 value between [minValue, maxValue).
        /// </summary>
        /// <param name="minValue">min value, default value is 0</param>
        /// <param name="maxValue">max value, default value is Int32.MaxValue</param>
        /// <returns>a random Int32 value</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">the minValue is greater than maxValue</exception>
        public static int GetRandomInt(int minValue = 0, int maxValue = Int32.MaxValue)
        {
            Contract.Assert(randLazy != null);
            return randLazy.Value.Next(minValue, maxValue);
        }


        #endregion
    }
}
