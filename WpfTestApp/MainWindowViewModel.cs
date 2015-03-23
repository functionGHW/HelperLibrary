/* 
 * FileName:    MainWindowViewModel.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/12/2015 2:23:57 PM
 * Version:     v1.0
 * Description:
 * */

namespace WpfTestApp
{
    using HelperLibrary.Core.Annotation;
    using HelperLibrary.WPF;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MainWindowViewModel : ViewModelBase
    {
        private string inputText;

        private const string ErrorScope = "WpfTestApp";

        //[LocalizedDisplay(ErrorScope, Name = "name")]
        [Display(Name = "name")]
        [LocalizedRequired(ErrorScope, ErrorMessage = "InputSomething")]
        public string InputText
        {
            get { return this.inputText; }
            set
            {
                if (this.inputText != value)
                {
                    this.inputText = value;
                    this.OnPropertyChanged(() => InputText);
                }
            }
        }
    }
}
