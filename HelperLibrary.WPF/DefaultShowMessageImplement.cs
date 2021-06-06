using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HelperLibrary.WPF
{
    public static class DefaultShowMessageImplement
    {
        /// <summary>
        /// show message using MessageBox
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool? UseMessageBox(ShowMessageContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var buttons = MessageBoxButton.OK;
            if (context.NeedConfirm)
            {
                buttons = context.ShowCancel ? MessageBoxButton.YesNoCancel : MessageBoxButton.YesNo;
            }
            else
            {
                if (context.ShowCancel)
                    buttons = MessageBoxButton.OKCancel;
            }
            return Application.Current.Dispatcher.Invoke(() =>
            {
                var dialogResult = MessageBox.Show(context.Message, context.Title, buttons);
                bool? result = null;
                switch (dialogResult)
                {
                    case MessageBoxResult.Yes:
                    case MessageBoxResult.OK:
                        result = true;
                        break;
                    case MessageBoxResult.No:
                        result = false;
                        break;
                    case MessageBoxResult.Cancel:
                        result = context.NeedConfirm ? null : (bool?)false;
                        break;
                }

                return result;
            });
        }

    }
}
