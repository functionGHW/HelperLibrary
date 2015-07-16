/* 
 * FileName:    DateTimeExtensions.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  7/15/2015 2:29:21 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.ExtensionHelper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class DateTimeExtensions
    {
        /// <summary>
        /// Check if the two datetimes is equals with timezone.
        /// If any of the datetime's Kind property is DateTimeKind.Unspecified,
        /// use it as a UTC time.
        /// </summary>
        /// <param name="dt">the datetime</param>
        /// <param name="another">datetime to compare</param>
        /// <returns>return true if the two datetimes are equal when they have the same DateTimeKind,
        /// otherwise, return false.</returns>
        public static bool EqualsWithTimeZone(this DateTime dt, DateTime another)
        {
            if (dt.Kind == another.Kind)
            {
                return dt.Ticks == another.Ticks;
            }

            return dt.ToLocalTime().Ticks == another.ToLocalTime().Ticks;
        }
    }
}
