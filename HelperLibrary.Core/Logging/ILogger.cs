/* 
 * FileName:    ILogger.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/1/2016 8:00:36 PM
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.Logging
{
    public interface ILogger
    {
        void Info(string message);

        void Debug(string message);

        void Warn(string message);

        void Error(string message);

        void Fatal(string message);
    }
}
