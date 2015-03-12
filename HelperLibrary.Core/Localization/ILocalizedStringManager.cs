/* 
 * FileName:    ILocalizedStringManager.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/11/2015 11:34:52 AM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ILocalizedStringManager
    {
        /// <summary>
        /// Get localized string.
        /// </summary>
        /// <param name="scope">scope. In most case, using name of assembly</param>
        /// <param name="key">the key for localizing</param>
        /// <param name="cultureName">culture name.</param>
        /// <returns>the localized string if successed, otherwise simply return the key string</returns>
        string GetLocalizedString(string scope, string key, string cultureName);
        
        /// <summary>
        /// Update the manager to reload resources
        /// </summary>
        void ReloadLocalizedStrings();
    }
}
