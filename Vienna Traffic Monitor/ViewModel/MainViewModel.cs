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

        public Scheduler<AbstractViewModel> Scheduler { get; private set; }

        public MainViewModel() {
            Scheduler = new Scheduler<AbstractViewModel>();
            Scheduler.Start();
            //Scheduler.Schedule(new UserControl1ViewModel());
            Scheduler.ScheduleInstant(new HauptfensterViewModel());
            Scheduler.AktuellChanged += OnSchedulerAktuellChanged;
        }

        private void OnSchedulerAktuellChanged(object Sender, EventArgs e) {
            RaisePropertyChangedEvent("Scheduler");
        }

    }

}
