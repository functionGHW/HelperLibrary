/* 
 * FileName:    IAppSettings.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/4/2016 5:37:10 PM
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
    /// manager configurations of a application
    /// </summary>
    public interface IAppSettings
    {
        string this[string name] { get;}

        string Get(string name);

        void Add(string name, string value);

        void Set(string name, string newValue);

        void Remove(string name);
    }
}
