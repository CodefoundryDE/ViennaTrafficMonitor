using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VtmFramework.Command;
using VtmFramework.Scheduler;
using VtmFramework.ViewModel;

namespace ViennaTrafficMonitor.ViewModel {

    public class MainViewModel : AbstractViewModel {

        public Scheduler<AbstractViewModel> Scheduler { get; private set; }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Objekte verwerfen, bevor Bereich verloren geht")]
        public MainViewModel() {
            Scheduler = new Scheduler<AbstractViewModel>();
            Scheduler.Schedule(AbfahrtenViewModelFactory.GetInstance(214461519));
            Scheduler.AktuellChanged += OnSchedulerAktuellChanged;
            Scheduler.Start();
        }

        private void OnSchedulerAktuellChanged(object Sender, EventArgs e) {
            RaisePropertyChangedEvent("Scheduler");
        }


        #region ButtonMap
        public ICommand ButtonMapCommand {
            get { return new AwaitableDelegateCommand(_switchToMap); }
        }

        private async Task _switchToMap() {
            Scheduler.ScheduleInstant(MapViewModelFactory.GetInstance());
        }
        #endregion



    }

}
