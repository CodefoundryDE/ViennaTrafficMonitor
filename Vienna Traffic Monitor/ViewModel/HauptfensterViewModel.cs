using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
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
            result = RaiseError("Exception", "Ganz toll geloggte Exception", EErrorButtons.OkCancel, new Exception("Logging-Exception zum Test2"));
            string text = (await result).ToString();
        }

        private string _flap;
        public string Flap { get { return _flap; } set { _flap = value; RaisePropertyChangedEvent("Flap"); } }

        public HauptfensterViewModel() : base() {
            Task.Run(() => {
                int delay = 250;
                while (true) {
                    Flap = "A";
                    Thread.Sleep(delay);
                    Flap = "B";
                    Thread.Sleep(delay);
                    Flap = "C";
                    Thread.Sleep(delay);
                    Flap = "D";
                    Thread.Sleep(delay);
                    Flap = "E";
                    Thread.Sleep(delay);
                }
            });
        }

    }

}
