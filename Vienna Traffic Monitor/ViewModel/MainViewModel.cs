using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VtmFramework.Scheduler;
using VtmFramework.ViewModel;

namespace ViennaTrafficMonitor.ViewModel {

    public class MainViewModel : AbstractViewModel {

        private Scheduler<AbstractViewModel> _scheduler;
        public Scheduler<AbstractViewModel> Scheduler {
            get { return _scheduler; }
            set { _scheduler = value; }
        }
        
        public MainViewModel() {
            Scheduler = new Scheduler<AbstractViewModel>();
            Scheduler.Schedule(new UserControl1ViewModel());
            Scheduler.Schedule(new HauptfensterViewModel());
            Scheduler.AktuellChanged += OnContentChanged;
            Scheduler.Start();
        }

        private void OnContentChanged(object Sender, EventArgs e) {
            RaisePropertyChangedEvent("Scheduler");
        }

    }

}
