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
using HelperLibrary.Core.ExtensionHelper;
using HelperLibrary.Core.Configurations;

namespace ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //NumberUtilityTest();
            //StringExtensionsTest();
            XmlConfigTest();

            Console.ReadKey();
        }

        private static void XmlConfigTest()
        {
            string filePath = @"E:\cfg.xml";
            IConfigurationFile cfg = new XmlConfigurationFile(filePath);
            cfg["time"] = DateTime.Now.ToString();

            Console.WriteLine(cfg["time"]);
            Console.WriteLine(cfg.GetConfiguration("time"));

            //throw exception
            cfg.UpdateConfiguration("update", "test");
        }

        private static void StringExtensionsTest()
        {
            string str = "abcdefg1235";
            Console.WriteLine("original string: " + str);
            Console.WriteLine("reverse string: " + str.ReverseString().FirstCharToUpper());
            Console.WriteLine("first char to upper:" + str.FirstCharToUpper());
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
