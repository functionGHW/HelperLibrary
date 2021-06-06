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

        private static readonly Dictionary<LogLevel, ConsoleColor> messageColors = new Dictionary<LogLevel, ConsoleColor>
        {
            {LogLevel.Debug, ConsoleColor.Gray },
            {LogLevel.Info, ConsoleColor.White },
            {LogLevel.Warn, ConsoleColor.DarkYellow },
            {LogLevel.Error, ConsoleColor.DarkRed },
            {LogLevel.Fatal, ConsoleColor.Red },
        };

        public void Log(LogLevel level, string message)
        {
            var oldColor = Console.ForegroundColor;
            if (messageColors.TryGetValue(level, out var color))
            {
                Console.ForegroundColor = color;
            }
            Console.WriteLine("[{0}] [{1}] {2}", DateTime.Now.ToString("HH:mm:ss"), level, message);
            Console.ForegroundColor = oldColor;
        }
    }
}
