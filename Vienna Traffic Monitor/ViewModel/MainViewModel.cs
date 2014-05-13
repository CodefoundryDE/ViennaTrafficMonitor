using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        [SuppressMessage("Microsoft.Reliability", "CA2000:Objekte verwerfen, bevor Bereich verloren geht")]
        public MainViewModel() {
            Scheduler = new Scheduler<AbstractViewModel>();
            Scheduler.Schedule(new HauptfensterViewModel());
            Scheduler.AktuellChanged += OnSchedulerAktuellChanged;
            Scheduler.Start();
            Scheduler.ScheduleInstant(MapViewModelFactory.GetInstance());
        }

        private void OnSchedulerAktuellChanged(object Sender, EventArgs e) {
            RaisePropertyChangedEvent("Scheduler");
        }

    }

}
