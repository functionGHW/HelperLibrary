/* 
 * FileName:    ColumnInfo.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  2017/2/14 11:41:55
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
    /// argument type of column info
    /// </summary>
    public class ColumnInfo
    {
        /// <summary>
        /// initialize instance
        /// </summary>
        /// <param name="index">index of column</param>
        /// <param name="name">name of column, usually use the contents of first row as columns</param>
        public ColumnInfo(int index, string name)
        {
            Index = index;
            Name = name;
        }

        /// <summary>
        /// get index of the column, 0-based, from left to right.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// get name of column
        /// </summary>
        public string Name { get; private set; }
    }
}
