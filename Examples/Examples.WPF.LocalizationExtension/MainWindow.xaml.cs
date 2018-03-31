using HelperLibrary.Core.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelperLibrary.WPF.LocalizationExtension;

namespace Examples.WPF.LocalizationExtension
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            // Binding Title to localization text
            this.SetLocalizationBinding(TitleProperty, "title", this.GetType().Assembly.GetName().Name);

            //this.Title = GetString("title");

        }


        private string GetString(string key)
        {
            return LocalizationUtility.GetString(key, this.GetType().Assembly.GetName().Name);
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!((sender as FrameworkElement)?.Tag is string culture))
                return;

            LocalizationHelper.RaiseCultureChanged(CultureInfo.GetCultureInfo(culture));
        }
    }
}
