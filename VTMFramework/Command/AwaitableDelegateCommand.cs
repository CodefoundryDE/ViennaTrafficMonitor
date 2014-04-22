using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VtmFramework.Command {

    public interface IAsyncCommand : ICommand {
        Task ExecuteAsync();
        ICommand Command { get; }
        void RaiseCanExecuteChanged();
    }

    public class AwaitableDelegateCommand : IAsyncCommand {

        private readonly Func<Task> _executeMethod;
        private readonly DelegateCommand _underlyingCommand;
        private bool _isExecuting;

        public AwaitableDelegateCommand(Func<Task> executeMethod) {
            _executeMethod = executeMethod;
            _underlyingCommand = new DelegateCommand(() => { });
        }

        public async Task ExecuteAsync() {
            try {
                _isExecuting = true;
                RaiseCanExecuteChanged();
                await _executeMethod();
            } finally {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public ICommand Command { get { return this; } }

        public bool CanExecute(object parameter) {
            return !_isExecuting && _underlyingCommand.CanExecute(parameter);
        }

        public event EventHandler CanExecuteChanged {
            add { _underlyingCommand.CanExecuteChanged += value; }
            remove { _underlyingCommand.CanExecuteChanged -= value; }
        }

        public async void Execute(object parameter) {
            await ExecuteAsync();
        }

        public void RaiseCanExecuteChanged() {
            _underlyingCommand.RaiseCanExecuteChanged();
        }
    }
}
