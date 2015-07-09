/* 
 * FileName:    ILocalizedStringLoader.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/11/2015 3:27:19 PM
 * Version:     v1.0
 * Description:
 * */


namespace HelperLibrary.Core.Localization
{
    using System.Collections.Generic;

    public interface ILocalizedStringLoader
    {
        /// <summary>
        /// read localized strings from somewhere
        /// </summary>
        /// <param name="scope">the scope</param>
        /// <param name="cultureName">the culture name, for example 'en-US'</param>
        /// <returns>a dictionary contains localized strings.</returns>
        IDictionary<string, string> GetLocalizedDictionary(string scope, string cultureName);
    }
}
