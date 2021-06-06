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

            
            
            try
            {
                //no logger setted, nothing happend
                LogTest();

                // log to Console
                // use ConsoleLogger
                ILogger logger = new ConsoleLogger();
                EnvironmentContext.SetLogger(logger);

                LogTest();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

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

            // test logging with format
            logger.Info("log info, time is {0}", DateTime.Now);

            // if no format args, logging the format string as a normal message.
            // so the char '{' and '}' are allowed.
            logger.Info("test char { and }");
        }
    }
}
