using HelperLibrary.Core.Localization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Examples.WPF.LocalizationExtension
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 在配置文件中指定，实际应用中可以通过其他手段设置
            string language = ConfigurationManager.AppSettings["language"];
            if (language != null && !"default".Equals(language, StringComparison.InvariantCultureIgnoreCase))
            {
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo(language);
            }

            Initialize();
        }

        private static void Initialize()
        {
            /*
             * 在项目中，应该在"解决方案资源管理器"中设置文件的属性：
             *      "复制到生成目录" 设置为 "始终复制"
             *      "生成操作" 设置为 "无"
             */
            string xmlFilePath = @"Localizations\Localization.xml";
            var xmlDocument = XDocument.Load(xmlFilePath);
            var dictionaryFactory = new XmlLocalizationDictionaryFactory(xmlDocument);
            ILocalizationDictionary chinese = dictionaryFactory.CreateLocalizationDictionary("zh-CN");
            ILocalizationDictionary english = dictionaryFactory.CreateLocalizationDictionary("en-US");

            var mngr = new LocalizationDictionaryManager();
            mngr.Add(chinese);
            mngr.Add(english);

            // 设置LocalizationUtility使用的字典管理器，此静态工具类通常在代码中使用
            LocalizationUtility.SetLocalizationDictionaryManager(mngr);

            // 设置本地化wpf扩展标记使用的字典管理器，此设置仅作用于wpf界面，通常和上面的保持一致。

            HelperLibrary.WPF.LocalizationExtension
                .LocalizationExtension.SetLocalizationDictionaryManager(mngr);
        }
    }
}
