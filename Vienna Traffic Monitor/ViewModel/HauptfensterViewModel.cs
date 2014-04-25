using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VtmFramework.Command;
using VtmFramework.Error;
using VtmFramework.ViewModel;

namespace ViennaTrafficMonitor.ViewModel {

    public class HauptfensterViewModel : AbstractViewModel {

        

        public ICommand BErrorCommand {
            get { return new AwaitableDelegateCommand(_berror); }
        }

        private async Task _berror() {
            Task<EErrorResult> result;
            //result = RaiseError("Hallo Welt!", "Ganz strenge Fehlermeldung!", EErrorButtons.OkCancel);
            result = RaiseError(new Exception("Logging-Exception zum Test2"), "Exception", "Ganz toll geloggte Exception", EErrorButtons.OkCancel);
            string text = (await result).ToString();
        }

        public HauptfensterViewModel() : base() {
            
        }

    }

}
