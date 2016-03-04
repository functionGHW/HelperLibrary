/* 
 * FileName:    Log4NetLogger.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/1/2016 8:11:33 PM
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace HelperLibrary.Core.Logging
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog logger;

        public Log4NetLogger(ILog logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            this.logger = logger;
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Warn(string message)
        {
            logger.Warn(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Fatal(string message)
        {
            logger.Fatal(message);
        }
    }
}
