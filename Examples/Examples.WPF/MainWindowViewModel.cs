/* 
 * FileName:    MainWindowViewModel.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  2/8/2017 9:56:58 PM
 * Description:
 * */

using HelperLibrary.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Examples.WPF
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Timer timer;
        private DateTime currentTime;
        private string message;

        public MainWindowViewModel()
        {
            ClickCommand = new SimpleCommand(ClickCommandAction);
            ConfirmCommand = new SimpleCommand(ConfirmCommandAction);

            timer = new Timer(TimeChanged, null, 0, 1000);
        }

        private void TimeChanged(object state)
        {
            CurrentTime = DateTime.Now;
        }


        public DateTime CurrentTime
        {
            get { return currentTime; }
            set { SetProperty(ref currentTime, value); }
        }

        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        public ICommand ClickCommand { get; set; }

        public ICommand ConfirmCommand { get; set; }

        private void ClickCommandAction()
        {
            ShowMessage("Current Second:" + DateTime.Now.Second);
        }

        private void ConfirmCommandAction()
        {
            var context = new ShowMessageContext
            {
                Title = "confirm dialog",
                Message = "are you sure to ...?",
                NeedConfirm = true,
                ShowCancel = true,
            };
            var result = ShowMessage(context);

            string selection = result.HasValue ? (result.Value ? "Yes" : "No") : "Cancel";
            Message = $"Your selection is {selection}.";
        }

    }
}
