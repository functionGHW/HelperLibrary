/* 
 * FileName:    Program.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/11/2015 10:19:04 AM
 * Version:     v1.0
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelperLibrary.Core;

namespace ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {

            NumberUtilityTest();

            Console.ReadKey();
        }


        private static void NumberUtilityTest()
        {
            // random int
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("random number:" + NumberUtility.GetRandomInt(-128, 127).ToString());
            }


            // hex string
            byte[] bytes = new byte[] { 20, 56, 9, 10, 11, 15, 16, 56 };
            Console.WriteLine("hex string:" + NumberUtility.BytesToHexString(bytes, true));

        }
    }
}
