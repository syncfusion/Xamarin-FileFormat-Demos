using System;
using System.Windows.Input;
namespace SampleBrowser
{
    /// <summary>
    /// Provides the base implementation for all the Command classes.
    /// </summary>
    public class CommandBase : SamplePage, ICommand
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBase"/> class. 
        /// </summary>
        public CommandBase()
        {
        }
        #endregion
        #region Events
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;
        #endregion
        #region Virtual Methods
        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed,
        /// this object can be set to null.
        /// </param>
        /// <returns>True if this command can be executed.</returns>
        public virtual bool CanExecuteCommand(object parameter)
        {
            return true;
        }
        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed,
        /// this object can be set to null.
        /// </param>
        protected virtual void ExecuteCommand(object parameter)
        {
        }
        #endregion
        #region Implementation
        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed,
        /// this object can be set to null.
        /// </param>
        /// <returns>True if this command can be executed.</returns>
        public bool CanExecute(object parameter)
        {
            return CanExecuteCommand(parameter);
        }
        /// <summary>
        /// Invoked when changes occur that affect whether or not the command should execute.
        /// </summary>
        internal void ExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed,
        /// this object can be set to null.
        /// </param>
        public void Execute(object parameter)
        {
            ExecuteCommand(parameter);
        }
        #endregion
    }
}