/* 
 * FileName:    LocalizedStringManager.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/11/2015 3:29:30 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Localization
{
    using IOAbstractions;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A default implementation of ILocalizedStringManger interface.
    /// </summary>
    public class LocalizedStringManager : ILocalizedStringManager
    {
        #region static members

        private static Lazy<LocalizedStringManager> lclStrMngrLazy =
            new Lazy<LocalizedStringManager>(
                () => new LocalizedStringManager(new XmlLocalizedStringLoader(new FileSystemWrapper()))
                );

        /// <summary>
        /// a default instance using a XmlLocalizedStringLoader as provider.
        /// </summary>
        public static LocalizedStringManager Default
        {
            get { return lclStrMngrLazy.Value; }
        }

        #endregion

        #region Fields

        private readonly Dictionary<string, IDictionary<string, string>> dictCache =
            new Dictionary<string, IDictionary<string, string>>();

        private readonly ILocalizedStringLoader localizedStringLoader;

        private static readonly object SyncObjectForLoadingDict = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize the LocalizedStringManager using XmlLocalizedStringLoader
        /// </summary>
        public LocalizedStringManager()
            : this(new XmlLocalizedStringLoader())
        {
        }

        /// <summary>
        /// Initialize the LocalizedStringManager with localizedStringLoader
        /// </summary>
        /// <param name="localizedStringLoader"></param>
        /// <exception cref="ArgumentNullException">localizedStringLoader is null</exception>
        public LocalizedStringManager(ILocalizedStringLoader localizedStringLoader)
        {
            if (localizedStringLoader == null)
                throw new ArgumentNullException(nameof(localizedStringLoader));

            this.localizedStringLoader = localizedStringLoader;
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
            Contract.Assert(this.dictCache != null);

            if (string.IsNullOrEmpty(scope))
                throw new ArgumentNullException(nameof(scope));

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (string.IsNullOrEmpty(cultureName))
            {
                cultureName = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            }

            // key for cache
            string dictName = scope + "." + cultureName;

            IDictionary<string, string> dict;
            if (!this.dictCache.TryGetValue(dictName, out dict))
            {
                // if not found in the cache, try to load using loader.
                if (!this.TryLoadDict(scope, cultureName))
                {
                    // load dict failed.
                    return key;
                }

                // get the dictionary after loading.
                if (!this.dictCache.TryGetValue(dictName, out dict))
                {
                    // this should not happend
                    return key;
                }
            }
            string localizedString;
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
            Contract.Assert(this.dictCache != null);
            // simply clean the cache.
            // all dictionnaries will be loaded again when they are reading.
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
            Contract.Assert(this.dictCache != null);

            if (this.localizedStringLoader == null)
                return false;

            lock (SyncObjectForLoadingDict)
            {
                string dictName = scope + "." + cultureName;

                // double checked
                if (!this.dictCache.ContainsKey(dictName))
                {
                    var dict = this.localizedStringLoader.GetLocalizedDictionary(scope, cultureName);
                    if (dict == null)
                    {
                        // loading resource failed.
                        return false;
                    }
                    else
                    {
                        this.dictCache.Add(dictName, dict);
                    }
                }

                return true;
            }
        }
    }
}
