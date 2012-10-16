using System;
using System.Windows.Input;
using System.Diagnostics;

namespace Ijv.Redstone.Input
{
    /// <summary>
    /// The execute command delegate.
    /// </summary>
    public delegate void ExecuteDelegateWithNoParameter();

    /// <summary>
    /// The can execute command delegate.
    /// </summary>
    public delegate bool CanExecuteDelegateWithNoParameter();

    /// <summary>
    /// The execute command delegate.
    /// </summary>
    /// <param name="parameter">The command parameter.</param>
    public delegate void ExecuteDelegateWithParameter(object parameter);

    /// <summary>
    /// The can execute command delegate.
    /// </summary>
    /// <param name="parameter">The command parameter.</param>
    /// <returns>True if the command can execute, otherwise returns false.</returns>
    public delegate bool CanExecuteDelegateWithParameter(object parameter);

    /// <summary>
    /// Represents a command object.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        /// <summary>
        /// The delegate that is called when command is invoked.
        /// </summary>
        private ExecuteDelegateWithNoParameter executeWithNoParameter;

        /// <summary>
        /// The delegate that determines whether the command can execute in its current state.
        /// </summary>
        private CanExecuteDelegateWithNoParameter canExecuteWithNoParameter;

        /// <summary>
        /// The delegate that is called when command is invoked.
        /// </summary>
        private ExecuteDelegateWithParameter executeWithParameter;

        /// <summary>
        /// The delegate that determines whether the command can execute in its current state.
        /// </summary>
        private CanExecuteDelegateWithParameter canExecuteWithParameter;

        /// <summary>
        /// Creates an in instance of the DelegateCommand class.
        /// </summary>
        /// <param name="executeDelegate">Delegate that is called when command is invoked.</param>
        public DelegateCommand(ExecuteDelegateWithNoParameter executeDelegate)
            : this(executeDelegate, null)
        {
        }

        /// <summary>
        /// Creates an in instance of the DelegateCommand class.
        /// </summary>
        /// <param name="executeDelegate">Delegate that is called when command is invoked.</param>
        public DelegateCommand(ExecuteDelegateWithParameter executeDelegate)
            : this(executeDelegate, null)
        {
        }

        /// <summary>
        /// Creates an in instance of the Command class.
        /// </summary>
        /// <param name="executeDelegate">Delegate that is called when command is invoked.</param>
        /// <param name="canExecuteDelegate">Delegate that determines whether the command can execute in its current state.</param>
        public DelegateCommand(ExecuteDelegateWithNoParameter executeDelegate, CanExecuteDelegateWithNoParameter canExecuteDelegate)
        {
            // preconditions

            Argument.IsNotNull("executeDelegate", executeDelegate);

            // implementation

            this.executeWithNoParameter = executeDelegate;
            this.canExecuteWithNoParameter = canExecuteDelegate;
            this.executeWithParameter = null;
            this.canExecuteWithParameter = null;
        }

        /// <summary>
        /// Creates an in instance of the Command class.
        /// </summary>
        /// <param name="executeDelegate">Delegate that is called when command is invoked.</param>
        /// <param name="canExecuteDelegate">Delegate that determines whether the command can execute in its current state.</param>
        public DelegateCommand(ExecuteDelegateWithParameter executeDelegate, CanExecuteDelegateWithParameter canExecuteDelegate)
        {
            // preconditions

            Argument.IsNotNull("executeDelegate", executeDelegate);

            // implementation

            this.executeWithParameter = executeDelegate;
            this.canExecuteWithParameter = canExecuteDelegate;
            this.executeWithNoParameter = null;
            this.canExecuteWithNoParameter = null;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>True if the command can be executed; otherwise false.</returns>
        public bool CanExecute(object parameter)
        {
            // assert that only one delegate or neither are used.
            Debug.Assert(this.canExecuteWithNoParameter == null || this.canExecuteWithParameter == null, "Only one of the can execute delegates can be non-null.");

            if (this.canExecuteWithNoParameter != null)
            {
                return this.canExecuteWithNoParameter();
            }
            else if (this.canExecuteWithParameter != null)
            {
                return this.canExecuteWithParameter(parameter);
            }

            return true;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            Debug.Assert(this.executeWithNoParameter == null ^ this.executeWithParameter == null, "Only one of the execute delegates can be non-null.");

            if (this.executeWithNoParameter != null)
            {
                this.executeWithNoParameter();
            }
            else if (this.executeWithParameter != null)
            {
                this.executeWithParameter(parameter);
            }
        }

        /// <summary>
        /// Causes the CanExecuteChanged event to be fired.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Causes the CanExecuteChanged event to be fired.
        /// </summary>
        public void Refresh()
        {
            EventHandler handler = this.CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
