using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.WPF
{
    public class ShowMessageContext
    {
        public string Title { get; set; }

        public string Message { get; set; }
        
        public bool NeedConfirm { get; set; }
        
        public bool ShowCancel { get; set; }

        public object ContextData { get; set; }

        public string YesText { get; set; }

        public string NoText { get; set; }

        public string CancelText { get; set; }

    }
}
