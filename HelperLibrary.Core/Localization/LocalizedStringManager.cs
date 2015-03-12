/* 
 * FileName:    LocalizedStringManager.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/11/2015 3:29:30 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LocalizedStringManager : ILocalizedStringManager
    {
        #region static members

        private static Lazy<LocalizedStringManager> lclStrMngrLazy =
            new Lazy<LocalizedStringManager>(
                () => new LocalizedStringManager(new XmlLocalizedStringLoader())
            );

        /// <summary>
        /// a default instance using a XmlLocalizedStringLoader as provider.
        /// </summary>
        public static LocalizedStringManager Default
        {
            get
            {
                return lclStrMngrLazy.Value;
            }
        }
        #endregion

        #region Fields

        private Dictionary<string, IDictionary<string, string>> dictCache =
            new Dictionary<string, IDictionary<string, string>>();

        private ILocalizedStringLoader localizedStringLoader;

        private static readonly object synObjectForLoadingDict = new object();

        #endregion

        /// <summary>
        /// Initialize the LocalizedStringManager with localizedStringLoader
        /// </summary>
        /// <param name="localizedStringLoader"></param>
        public LocalizedStringManager(ILocalizedStringLoader localizedStringLoader)
        {
            this.localizedStringLoader = localizedStringLoader;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the LocalizedStringLoader
        /// </summary>
        public ILocalizedStringLoader LocalizedStringLoader
        {
            get { return localizedStringLoader; }
            set { localizedStringLoader = value; }
        }

        #endregion

        #region ILocalizedStringManager Members

        /// <summary>
        /// Get localized string.
        /// </summary>
        /// <param name="scope">scope. In most case, using name of assembly</param>
        /// <param name="key">the key for localizing</param>
        /// <param name="cultureName">culture name. if not given using the current thread culture</param>
        /// <returns>the localized string if successed, otherwise simply return the key string</returns>
        public string GetLocalizedString(string scope, string key, string cultureName)
        {
            Contract.Assert(dictCache != null);

            if (string.IsNullOrEmpty(scope))
                throw new ArgumentNullException("scope");

            if (string.IsNullOrEmpty(cultureName))
            {
                cultureName = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            }

            // key for cache
            string dictName = scope + "." + cultureName;

            IDictionary<string, string> dict = null;
            if (!dictCache.TryGetValue(dictName, out dict))
            {
                // if not found in the cache, try to load using loader.
                if (!TryLoadDict(scope, cultureName))
                {
                    // load dict failed.
                    return key;
                }

                // get the dictionary after loading.
                if (!dictCache.TryGetValue(dictName, out dict))
                {
                    // this should not happend
                    return key;
                }
            }
            string localizedString = null;
            if (dict.TryGetValue(key, out localizedString))
            {
                return localizedString;
            }
            else
            {
                return key;
            }
        }

        /// <summary>
        /// Update the manager to reload resources
        /// </summary>
        public void ReloadLocalizedStrings()
        {
            Contract.Assert(dictCache != null);
            // simply clean the cache.
            // then all dictionnary with load again when they are read.
            this.dictCache.Clear();
        }

        #endregion

        /// <summary>
        /// load the dictionary to the cache.
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="cultureName"></param>
        /// <returns></returns>
        private bool TryLoadDict(string scope, string cultureName)
        {
            Contract.Assert(dictCache != null);

            if (localizedStringLoader == null)
                return false;

            lock (synObjectForLoadingDict)
            {
                string dictName = scope + "." + cultureName;

                // double checked
                if (!dictCache.ContainsKey(dictName))
                {
                    var dict = localizedStringLoader.GetLocalizedDictionary(scope, cultureName);
                    if (dict == null)
                    {
                        // loading resource failed.
                        return false;
                    }
                    else
                    {
                        dictCache.Add(dictName, dict);
                    }
                }

                return true;
            }
        }
    }
}
