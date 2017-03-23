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
        private readonly ILog infoLogger;
        private readonly ILog debugLogger;
        private readonly ILog warnLogger;
        private readonly ILog errorLogger;
        private readonly ILog fatalLogger;
        
        public Log4NetLogger(ILog logger) : this(logger, logger, logger, logger, logger)
        {
        }

        public Log4NetLogger(ILog infoLogger, ILog debugLogger, ILog warnLogger, ILog errorLogger, ILog fatalLogger)
        {
            if (infoLogger == null)
                throw new ArgumentNullException(nameof(infoLogger));
            if (debugLogger == null)
                throw new ArgumentNullException(nameof(debugLogger));
            if (warnLogger == null)
                throw new ArgumentNullException(nameof(warnLogger));
            if (errorLogger == null)
                throw new ArgumentNullException(nameof(errorLogger));
            if (fatalLogger == null)
                throw new ArgumentNullException(nameof(fatalLogger));

            this.infoLogger = infoLogger;
            this.debugLogger = debugLogger;
            this.warnLogger = warnLogger;
            this.errorLogger = errorLogger;
            this.fatalLogger = fatalLogger;
        }
        
        public void Log(LogLevel level, string message)
        {
            switch(level)
            {
                case LogLevel.Info:
                    infoLogger.Info(message);
                    break;
                case LogLevel.Debug:
                    infoLogger.Debug(message);
                    break;
                case LogLevel.Warn:
                    infoLogger.Warn(message);
                    break;
                case LogLevel.Error:
                    infoLogger.Error(message);
                    break;
                case LogLevel.Fatal:
                    infoLogger.Fatal(message);
                    break;
            }
        }
    }
}
