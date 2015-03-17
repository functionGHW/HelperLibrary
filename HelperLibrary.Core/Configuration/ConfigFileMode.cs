/* 
 * FileName:    ConfigFileMode.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/17/2015 2:44:29 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public enum ConfigFileMode
    {
        /// <summary>
        /// 
        /// </summary>
        Open = 1, 

        Create = 2,

        OpenOrCreate = 3,

        CreateNew = 4
    }
}
