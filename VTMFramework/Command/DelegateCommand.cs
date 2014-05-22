using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace VtmFramework.Command {

    public class DelegateCommand : ICommand {

        private readonly Action<object> _action;

        public DelegateCommand(Action<object> action) {
            _action = action;
        }

        public DelegateCommand(Action action) {
            // Hack, weil jetzt Actions mit Parametern benötigt werden
            _action = new Action<object>((object parameter) => {
                action();
            });
        }

        public void Execute(object parameter) {
            _action(parameter);
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged() {
            var handler = CanExecuteChanged;
            if (handler != null)
                handler(this, new EventArgs());
        }
    }
}
