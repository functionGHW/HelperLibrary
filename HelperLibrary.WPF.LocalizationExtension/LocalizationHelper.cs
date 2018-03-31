using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using HelperLibrary.Core.Localization;

namespace HelperLibrary.WPF.LocalizationExtension
{
    /// <summary>
    /// some static methods
    /// </summary>
    public static class LocalizationHelper
    {

        private static ILocalizationDictionaryManager locDictMngr = EmptyLocalizationDictionaryManager.Instance;

        /// <summary>
        /// 设置使用的本地化字典管理器，初始默认的实例为DoNothingLocalizationManager.Instance
        /// </summary>
        /// <param name="localizationDictionaryManager">本地化管理实例</param>
        public static void SetLocalizationDictionaryManager(ILocalizationDictionaryManager localizationDictionaryManager)
        {
            if (localizationDictionaryManager == null)
                throw new ArgumentNullException(nameof(localizationDictionaryManager));
            locDictMngr = localizationDictionaryManager;
        }

        public static ILocalizationDictionaryManager LocalizationDictionaryManager
        {
            get { return locDictMngr; }
        }


        /// <summary>
        /// Create a Binding of localization text
        /// </summary>
        /// <param name="key"></param>
        /// <param name="scope"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static Binding CreateLocalizationBinding(string key, string scope, CultureInfo culture = null)
        {
            var bindingText = new LocalizationText(key, scope, LocalizationDictionaryManager)
            {
                Culture = culture
            };
            var binding = new Binding(nameof(bindingText.Value))
            {
                Source = bindingText,
                IsAsync = true
            };
            return binding;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SubscribeCultureChanged(Action<CultureInfo> callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            var target = callback.Target;
            var weakRef = target == null ? null : new WeakReference(target);
            var item = new CallbackItem(weakRef, callback.Method);

            itemList.TryAdd(item, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void UnsubscribeCultureChanged(Action<CultureInfo> callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            var target = callback.Target;
            CallbackItem removeItem;
            var keys = itemList.Keys.ToArray();
            if (target == null)
            {
                removeItem = keys.FirstOrDefault(item => item.Instance == null
                                                         && item.Method == callback.Method);
            }
            else
            {
                removeItem = keys.FirstOrDefault(item => item.Instance != null
                                            && item.Instance.Target == target
                                            && item.Method == callback.Method);
            }

            if (removeItem != null)
                itemList.TryRemove(removeItem, out var tmp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="culture"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void RaiseCultureChanged(CultureInfo culture)
        {
            if (culture == null)
                throw new ArgumentNullException(nameof(culture));

            var list = itemList.Keys.ToArray();
            object[] parameters = { culture };
            foreach (var item in list)
            {
                if (item.Instance == null)
                {
                    item.Method.Invoke(null, parameters);
                }
                else
                {
                    var target = item.Instance.Target;
                    if (target == null)
                    {
                        itemList.TryRemove(item, out var _);
                    }
                    else
                    {
                        item.Method.Invoke(target, parameters);
                    }
                }
            }
        }


        private static ConcurrentDictionary<CallbackItem, int> itemList = new ConcurrentDictionary<CallbackItem, int>();


        private class CallbackItem
        {
            public CallbackItem(WeakReference instance, MethodInfo method)
            {
                Instance = instance;
                Method = method;
            }

            public WeakReference Instance;

            public MethodInfo Method;
        }
    }
}
