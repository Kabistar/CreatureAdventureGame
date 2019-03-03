using System;
using System.Windows.Input;

namespace CreatureGameMapEditor.ViewModels
{
    /// <summary>
    /// Command that simply executes an action.
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// Event fired when we can exist <see cref="CanExecute(object)"/> value has changed
        /// </summary>
        public event EventHandler CanExecuteChanged = (sending, e) => { };

        private Action action;

        public RelayCommand(Action action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action();
        }
    }
}
