/* 
 * FileName:    LoggerExtensions.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  5/10/2016 10:34:25 PM
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.Logging
{
    public static class LoggerExtensions
    {
        public static void Log(this ILogger logger, LogLevel level, string format, params object[] args)
        {

            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            string message = args.Length == 0 ? format : string.Format(format, args);
            logger.Log(level, message);
        }
        
        public static void Info(this ILogger logger, string format, params object[] args)
        {
            Log(logger, LogLevel.Info, format, args);
        }

        public static void Debug(this ILogger logger, string format, params object[] args)
        {
            Log(logger, LogLevel.Debug, format, args);
        }

        public static void Warn(this ILogger logger, string format, params object[] args)
        {
            Log(logger, LogLevel.Warn, format, args);
        }

        public static void Error(this ILogger logger, string format, params object[] args)
        {
            Log(logger, LogLevel.Error, format, args);
        }

        public static void Fatal(this ILogger logger, string format, params object[] args)
        {
            Log(logger, LogLevel.Fatal, format, args);
        }
    }
}
