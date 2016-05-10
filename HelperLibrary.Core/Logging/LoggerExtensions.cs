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
        public static void Info(this ILogger logger, string format, params object[] args)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            string message = string.Format(format, args);
            logger.Info(message);
        }

        public static void Debug(this ILogger logger, string format, params object[] args)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            string message = string.Format(format, args);
            logger.Debug(message);
        }

        public static void Warn(this ILogger logger, string format, params object[] args)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            string message = string.Format(format, args);
            logger.Warn(message);
        }

        public static void Error(this ILogger logger, string format, params object[] args)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            string message = string.Format(format, args);
            logger.Error(message);
        }

        public static void Fatal(this ILogger logger, string format, params object[] args)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            string message = string.Format(format, args);
            logger.Fatal(message);
        }
    }
}
