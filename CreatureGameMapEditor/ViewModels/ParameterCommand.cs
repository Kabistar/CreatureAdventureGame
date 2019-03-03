using System;
using System.Windows.Input;

namespace CreatureGameMapEditor.ViewModels
{
    /// <summary>
    /// Command that simply executes an action.
    /// </summary>
    public class ParameterCommand : ICommand
    {
        /// <summary>
        /// Event fired when we can exist <see cref="CanExecute(object)"/> value has changed
        /// </summary>
        public event EventHandler CanExecuteChanged = (sending, e) => { };

        private Action<object> action;

        public ParameterCommand(Action<object> action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action(parameter);
        }
    }
}
