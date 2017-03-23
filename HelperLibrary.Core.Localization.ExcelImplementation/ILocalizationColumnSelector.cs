/* 
 * FileName:    ILocalizationColumnSelector.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  2017/2/14 11:40:59
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
    /// 
    /// </summary>
    public interface ILocalizationColumnSelector
    {
        /// <summary>
        /// determine if the column is key column
        /// </summary>
        /// <param name="arg">column info</param>
        /// <returns>true for yes, otherwise false</returns>
        bool IsKeyColumn(ColumnInfo arg);

        /// <summary>
        /// determine if the column is specified culture column
        /// </summary>
        /// <param name="cultureName">culture name</param>
        /// <param name="arg">column info</param>
        /// <returns>true for yes, otherwise false</returns>
        bool IsCultureColumn(string cultureName, ColumnInfo arg);

    }
}
