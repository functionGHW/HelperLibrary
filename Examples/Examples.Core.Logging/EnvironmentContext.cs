/* 
 * FileName:    EnvironmentContext.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  2017/2/20 16:57:23
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
    public static class EnvironmentContext
    {
        /// <summary>
        /// Global logger, use EmptyLogger as default value so we don't have to do null-checking.
        /// </summary>
        public static ILogger Logger { get; private set; } = new EmptyLogger();

        /// <summary>
        /// change 
        /// </summary>
        /// <param name="newLogger"></param>
        public static void SetLogger(ILogger newLogger)
        {
            if (newLogger == null)
                throw new ArgumentNullException(nameof(newLogger));

            Logger = newLogger;
        }
    }
}
