/* 
 * FileName:    SingleRunningCommand.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  8/16/2015 10:02:54 AM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.WPF.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Core;

    public class SingleRunningCommand : ICommand
    {
        #region private fields

        private readonly SingleRunningAction action;

        private readonly object actionSyncObj = new object();

        private bool isExecuting;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize instance of SimpleCommand
        /// </summary>
        /// <param name="action">the action to execute when Command was called</param>
        /// <exception cref="ArgumentNullException">action is null</exception>
        public SingleRunningCommand(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            this.action = new SingleRunningAction(action);
        }

        private bool IsExecuting
        {
            get { return this.isExecuting; }
            set
            {
                if (value == this.isExecuting)
                    return;

                this.isExecuting = value;
                // notify the canExecuteIsChanged
                EventHandler onCanExecuteChanged = this.CanExecuteChanged;
                if (onCanExecuteChanged != null)
                    onCanExecuteChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return !this.IsExecuting;
        }

        public event EventHandler CanExecuteChanged;

        public async void Execute(object parameter)
        {
            if (this.action.IsRunning)
                return;
            try
            {
                this.IsExecuting = true;
                await this.action.InvokeAsync();
            }
            finally
            {
                this.IsExecuting = false;
            }
        }

        #endregion
    }
}
