/* 
 * FileName:    IWindowProperty.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  4/8/2015 11:38:10 AM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.WPF.ExtensionInterfaces
{
    using System.Windows;

    /// <summary>
    /// This interface is used to ensure ViewModel class hold a property of System.Windows.Window,
    /// which it may associate with.
    /// </summary>
    public interface IWindowProperty
    {
        /// <summary>
        /// Gets or sets System.Windows.Window which the instance associate with
        /// </summary>
        Window Window { get; set; }
    }
}
