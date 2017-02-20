using HelperLibrary.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Core.Logging
{
    class Program
    {
        static void Main(string[] args)
        {

            // nothing happend
            LogTest();

            // use ConsoleLogger
            ILogger logger = new ConsoleLogger();
            EnvironmentContext.SetLogger(logger);

            // log to Console
            LogTest();

            //// use log4net
            //logger = new Log4NetLogger(null);
            //EnvironmentContext.SetLogger(logger);

            //// log to log4net
            //LogTest();

            Console.Write("Press ENTER to exit...");
            Console.ReadLine();
        }

        static void LogTest()
        {
            var logger = EnvironmentContext.Logger;

            logger.Info("log info message");
            logger.Debug("log debug message");
            logger.Warn("log warn message");
            logger.Error("log error message");
            logger.Fatal("log fatal message");
        }
    }
}
