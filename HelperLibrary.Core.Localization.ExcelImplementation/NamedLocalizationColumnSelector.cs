/* 
 * FileName:    NamedLocalizationColumnSelector.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  2017/2/14 11:41:24
 * Version:     v1.0
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.Localization
{
    /// <summary>
    /// simple implementation of ILocalizationColumnSelector, use column name to identify
    /// </summary>
    public class NamedLocalizationColumnSelector : ILocalizationColumnSelector
    {
        /// <summary>
        /// the default instance
        /// </summary>
        public static readonly NamedLocalizationColumnSelector Instance = new NamedLocalizationColumnSelector();

        private NamedLocalizationColumnSelector()
        {
        }

        /// <summary>
        /// determine if the column is key column. 
        /// this implementation works by checking whether the column name equals "key"(ignore case)
        /// </summary>
        /// <param name="arg">column info</param>
        /// <returns>true for yes, otherwise false</returns>
        public bool IsKeyColumn(ColumnInfo arg)
        {
            if (arg == null)
                throw new ArgumentNullException(nameof(arg));

            return "key".Equals(arg.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// determine if the column is specified culture column.
        /// this implementation works by checking whether the column name equals cultureName(ignore case)
        /// </summary>
        /// <param name="cultureName">culture name</param>
        /// <param name="arg">column info</param>
        /// <returns>true for yes, otherwise false</returns>
        public bool IsCultureColumn(string cultureName, ColumnInfo arg)
        {
            if (cultureName == null)
                throw new ArgumentNullException(nameof(cultureName));
            if (arg == null)
                throw new ArgumentNullException(nameof(arg));

            return cultureName.Equals(arg.Name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
