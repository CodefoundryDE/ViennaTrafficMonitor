using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ViennaTrafficMonitor.Events;
using VtmFramework.Command;
using VtmFramework.Scheduler;
using VtmFramework.ViewModel;

namespace ViennaTrafficMonitor.ViewModel {

    public class MainViewModel : AbstractViewModel {

        public SucheViewModel Suche { get; private set; }

        public Scheduler<AbstractViewModel> Scheduler { get; private set; }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Objekte verwerfen, bevor Bereich verloren geht")]
        public MainViewModel() {
            Suche = new SucheViewModel();
            Suche.SucheSubmitted += _sucheSubmitted;

            Scheduler = new Scheduler<AbstractViewModel>();
            Scheduler.ScheduleInstant(new HauptfensterViewModel());
            //Scheduler.Schedule(AbfahrtenViewModelFactory.GetInstance(214461519));
            Scheduler.AktuellChanged += OnSchedulerAktuellChanged;
            Scheduler.Start();
        }

        private void OnSchedulerAktuellChanged(object Sender, EventArgs e) {
            RaisePropertyChangedEvent("Scheduler");
        }

        private void _sucheSubmitted(SucheEventArgs e) {
            Scheduler.ScheduleInstant(AbfahrtenViewModelFactory.GetInstance(e.HaltestelleSelected));
        }


        #region ButtonMap
        public ICommand ButtonMapCommand {
            get { return new DelegateCommand(_switchToMap); }
        }
        private void _switchToMap() {
            Scheduler.ScheduleInstant(MapViewModelFactory.GetInstance());
        }
        #endregion



    }

}
