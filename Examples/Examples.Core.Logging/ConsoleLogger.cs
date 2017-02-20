/* 
 * FileName:    ConsoleLogger.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  2017/2/20 16:48:23
 * Version:     v1.0
 * Description:
 * */

using HelperLibrary.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Core.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Log(LogLevel level, string message)
        {
            var oldColor = Console.ForegroundColor;
            switch (level)
            {
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Warn:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case LogLevel.Fatal:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }

            Console.WriteLine("[{0}] [{1}] {2}", DateTime.Now.ToString("HH:mm:ss"), level, message);
            Console.ForegroundColor = oldColor;
        }
    }
}
