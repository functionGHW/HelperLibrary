using HelperLibrary.Core.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Examples.Localization.XmlImplementation
{
    class Program
    {
        static void Main(string[] args)
        {
            Initialize();
            string scope = typeof(Program).Assembly.GetName().Name;
            
            string hello = LocalizationUtility.GetString("hello", scope, "zh-CN");
            Console.WriteLine("zh-CN: {0}", hello);
            Console.WriteLine();
            
            hello = LocalizationUtility.GetString("hello", scope, "en-US");
            Console.WriteLine("en-US: {0}", hello);
            Console.WriteLine();
            
            // 未指定cultureName，自动使用默认UI设置
            hello = LocalizationUtility.GetString("hello", scope);
            Console.WriteLine("default UI culture: {0}", hello);
            Console.WriteLine();
            
            Console.Write("按Enter键退出");
            Console.ReadLine();
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

            LocalizationUtility.SetLocalizationDictionaryManager(mngr);
        }
    }
}
