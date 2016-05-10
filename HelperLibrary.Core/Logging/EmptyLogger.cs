/* 
 * FileName:    EmptyLogger.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  5/10/2016 10:54:24 PM
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.Logging
{
    /// <summary>
    /// a logger that do nothing, use it as a Null-Object.
    /// </summary>
    public class EmptyLogger : ILogger
    {
        public void Info(string message)
        {
        }

        public void Debug(string message)
        {
        }

        public void Warn(string message)
        {
        }

        public void Error(string message)
        {
        }

        public void Fatal(string message)
        {
        }
    }
}
