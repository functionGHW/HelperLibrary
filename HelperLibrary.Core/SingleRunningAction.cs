/* 
 * FileName:    SingleRunningAction.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/11/2015 5:26:02 PM
 * Version:     v1.0
 * Description: This class is designed for invoking an action 
 *              that only one of threads will invoke the action at a time.
 * */

namespace HelperLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class SingleRunningAction
    {

        #region Fields

        private readonly object syncObj = new object();

        #endregion

        /// <summary>
        /// Initialized the instance
        /// </summary>
        /// <param name="action">the action to invoke</param>
        public SingleRunningAction(Action action)
        {
            this.WrappedAction = action;
            this.IsRunning = false;
        }

        #region Properties

        /// <summary>
        /// Get or set the wrapped action
        /// </summary>
        public Action WrappedAction { get; set; }

        /// <summary>
        /// Is the action running
        /// </summary>
        public bool IsRunning { get; private set; }

        #endregion

        /// <summary>
        /// Invoke the action.
        /// </summary>
        /// <returns>true if invoke successfully, false if the action is running.</returns>
        public bool Invoke()
        {
            Contract.Ensures(syncObj != null);

            /* use a lock to make sure that there is at most 
             * one thread runs this action at any time.
             */
            bool isEntered = Monitor.TryEnter(syncObj);

            if (!isEntered)
            {
                // the action is running, stop invoking.
                return false;
            }

            try
            {
                IsRunning = true;
                if (WrappedAction != null)
                {
                    WrappedAction.Invoke();
                }
                return true;
            }
            finally
            {
                if (Monitor.IsEntered(syncObj))
                {
                    Monitor.Exit(syncObj);
                }
                IsRunning = false;
            }
        }

        /// <summary>
        /// Invoke the action asynchronously.
        /// </summary>
        /// <returns>true if invoke successfully, false if the action is running.</returns>
        public Task<bool> InvokeAsync()
        {
            return Task.Run(() => Invoke());
        }
    }
}
