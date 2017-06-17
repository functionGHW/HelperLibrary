/* 
 * FileName:    AppSettingsExtension.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  2/9/2017 15:10:46
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.Configurations
{
    /// <summary>
    /// Extension methods for IAppSettings
    /// </summary>
    public static class AppSettingsExtension
    {
        /// <summary>
        /// Get int value from configurations, throw exceptions if error.
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetIntValue(this IAppSettings appSettings, string name)
        {
            if (appSettings == null)
                throw new ArgumentNullException(nameof(appSettings));

            string value = appSettings.Get(name);
            return int.Parse(value);
        }

        /// <summary>
        /// Try get int value from configurations
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns>true if success, false if error.</returns>
        public static bool TryGetIntValue(this IAppSettings appSettings, string name, out int value)
        {
            if (appSettings == null)
                throw new ArgumentNullException(nameof(appSettings));

            string valueStr = appSettings.Get(name);
            return int.TryParse(valueStr, out value);
        }

        /// <summary>
        /// Get double value from configurations, throw exceptions if error.
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static double GetdoubleValue(this IAppSettings appSettings, string name)
        {
            if (appSettings == null)
                throw new ArgumentNullException(nameof(appSettings));

            string value = appSettings.Get(name);
            return double.Parse(value);
        }

        /// <summary>
        /// Try get double value from configurations
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns>true if success, false if error.</returns>
        public static bool TryGetDoubleValue(this IAppSettings appSettings, string name, out double value)
        {
            if (appSettings == null)
                throw new ArgumentNullException(nameof(appSettings));

            string valueStr = appSettings.Get(name);
            return double.TryParse(valueStr, out value);
        }
    }
}
