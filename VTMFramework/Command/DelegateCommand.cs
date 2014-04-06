using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

<<<<<<< HEAD:VTMFramework/src/DelegateCommand.cs
namespace ViennaTrafficMonitor {
=======
namespace VtmFramework.Command {
>>>>>>> 2ae897d472c322c3cc45e04637dc816ec972ee0d:VTMFramework/Command/DelegateCommand.cs

    public class DelegateCommand : ICommand {

        private readonly Action _action;

        public DelegateCommand(Action action) {
            _action = action;
        }

        public void Execute(object parameter) {
            _action();
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
