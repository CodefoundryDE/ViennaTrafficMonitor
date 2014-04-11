using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VtmFramework.ViewModel;

namespace ViennaTrafficMonitor.ViewModel {

    public class MainViewModel : AbstractViewModel {

        /* Man könnte hier über ein Strategie-Muster verschiedene Animationen zum Bildschirm-Wechseln einhängen */

        private AbstractViewModel _activeViewModel;

        public AbstractViewModel ActiveViewModel {
            get { return _activeViewModel; }
            set { _activeViewModel = value; RaisePropertyChangedEvent("ActiveViewModel"); }
        }

        public MainViewModel() {
            ActiveViewModel = new HauptfensterViewModel();

            /* DEMO-Code */
            Task task = new Task(new Action(() => {
                while (true) {
                    ActiveViewModel = new UserControl1ViewModel();
                    Thread.Sleep(5000);
                    ActiveViewModel = new HauptfensterViewModel();
                    Thread.Sleep(5000);
                }
            }));
            task.Start();
        }

    }

}
