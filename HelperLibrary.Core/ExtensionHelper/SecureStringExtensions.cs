/* 
 * FileName:    SecureStringExtensions.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  4/4/2018 11:29:10 AM
 * Version:     v1.0
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.ExtensionHelper
{
    public static class SecureStringExtensions
    {
        /// <summary>
        /// Read secure string as char array
        /// </summary>
        /// <param name="secureStr"></param>
        /// <returns></returns>
        public static char[] ReadChars(this SecureString secureStr)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                var result = new char[secureStr.Length];
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(secureStr);
                for (int i = 0; i < secureStr.Length; i++)
                {
                    short unicodeChar = Marshal.ReadInt16(valuePtr, i * 2);
                    result[i] = (char)unicodeChar;
                }
                return result;
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        /// <summary>
        /// Read secure string content
        /// </summary>
        /// <param name="secureStr"></param>
        /// <returns></returns>
        public static string ReadString(this SecureString secureStr)
        {
            var chars = ReadChars(secureStr);
            string str = new string(chars);
            Array.Clear(chars, 0, chars.Length);
            return str;
        }
    }
}
