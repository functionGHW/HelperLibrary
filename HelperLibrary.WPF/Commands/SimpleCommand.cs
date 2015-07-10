/* 
 * FileName:    SimpleCommand.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  6/28/2015 3:21:15 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.WPF.Commands
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// provide easy to use implementation of ICommand.
    /// </summary>
    public class SimpleCommand : ICommand
    {
        #region private fields

        private readonly Action action;
        private readonly Func<bool> canCmdExecute;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize instance of SimpleCommand
        /// </summary>
        /// <param name="action">the action to execute when Command was called</param>
        /// <exception cref="ArgumentNullException">action is null</exception>
        public SimpleCommand(Action action)
            : this(action, null)
        {
        }

        /// <summary>
        /// Initialize instance of SimpleCommand
        /// </summary>
        /// <param name="action">the action to execute when Command was called</param>
        /// <param name="canCmdExecute">a function to check if the Command can execute</param>
        /// <exception cref="ArgumentNullException">action is null</exception>
        public SimpleCommand(Action action, Func<bool> canCmdExecute)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            this.action = action;
            this.canCmdExecute = canCmdExecute;
        }

        #endregion

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (this.canCmdExecute == null)
            {
                return true;
            }
            else
            {
                return this.canCmdExecute();
            }
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this.action();
        }

        #endregion

        public void OnCanExecuteChanged()
        {
            EventHandler onCanExecuteChanged = this.CanExecuteChanged;
            if (onCanExecuteChanged != null)
                onCanExecuteChanged(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// provide easy to use implementation of ICommand.
    /// </summary>
    public class SimpleCommand<T> : ICommand
    {
        #region private fields

        private readonly Action<T> action;
        private readonly Func<T, bool> canCmdExecute;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize instance of SimpleCommand
        /// </summary>
        /// <param name="action">the action to execute when Command was called</param>
        /// <exception cref="ArgumentNullException">action is null</exception>
        public SimpleCommand(Action<T> action)
            : this(action, null)
        {
        }

        /// <summary>
        /// Initialize instance of SimpleCommand
        /// </summary>
        /// <param name="action">the action to execute when Command was called</param>
        /// <param name="canCmdExecute">a function to check if the Command can execute</param>
        /// <exception cref="ArgumentNullException">action is null</exception>
        public SimpleCommand(Action<T> action, Func<T, bool> canCmdExecute)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            this.action = action;
            this.canCmdExecute = canCmdExecute;
        }

        #endregion

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (this.canCmdExecute == null)
            {
                return true;
            }
            if (parameter is T)
            {
                return this.canCmdExecute((T)parameter);
            }
            else
            {
                return false;
            }
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (parameter is T)
            {
                this.action((T)parameter);
            }
            else
            {
                this.action(default(T));
            }
        }

        #endregion

        public void OnCanExecuteChanged()
        {
            EventHandler onCanExecuteChanged = this.CanExecuteChanged;
            if (onCanExecuteChanged != null)
                onCanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
