using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ViennaTrafficMonitor.Events;
using ViennaTrafficMonitor.Mapper;
using VtmFramework.Command;
using VtmFramework.Error;
using VtmFramework.Error.Exceptions;
using VtmFramework.Scheduler;
using VtmFramework.ViewModel;

namespace ViennaTrafficMonitor.ViewModel {

    public class MainViewModel : AbstractViewModel {

        public HauptfensterViewModel Hauptfenster { get; private set; }
        public SucheViewModel Suche { get; private set; }
        public MapViewModel Map { get; private set; }
        public EinstellungenViewModel Einstellungen { get; private set; }

        private ConcurrentDictionary <AbstractViewModel, ErrorViewModel> _errors;
        public ErrorViewModel Error {
            get { return _errors.ElementAtOrDefault(0).Value; }
        }


        public Scheduler<AbstractViewModel> Scheduler { get; private set; }

        
        public MainViewModel() {
            _loadTheme();
            _errors = new ConcurrentDictionary<AbstractViewModel, ErrorViewModel>();
            Scheduler = new Scheduler<AbstractViewModel>();
            Scheduler.AktuellChanged += OnSchedulerAktuellChanged;            
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Objekte verwerfen, bevor Bereich verloren geht")]
        private void _initializeView() {
            Einstellungen = new EinstellungenViewModel();
            Einstellungen.Beenden += OnBeenden;
            Einstellungen.Info += OnInfo;
            _registerEvents(Einstellungen);
            RaisePropertyChangedEvent("Einstellungen");

            Suche = SucheViewModelFactory.Instance;
            Suche.SucheSubmitted += OnSucheSubmitted;
            _registerEvents(Suche);
            RaisePropertyChangedEvent("Suche");

            Map = MapViewModelFactory.Instance;
            Map.HaltestelleSelected += OnSucheSubmitted;
            _registerEvents(Map);

            Hauptfenster = HauptfensterViewModelFactory.Instance;
            _registerEvents(Hauptfenster);            
            
            Scheduler.ScheduleInstant(Hauptfenster);
        }

        public void initializeApp() {
            InitializationViewModel Ivm = new InitializationViewModel();
            Ivm.Beenden += OnBeenden;
            Ivm.Initialized += OnInitialized;
            _registerEvents(Ivm);
            Scheduler.ScheduleInstant(Ivm);
        }

        private void OnSchedulerAktuellChanged(object Sender, EventArgs e) {
            RaisePropertyChangedEvent("Scheduler");
        }

        private static void _loadTheme() {
            string theme = Properties.Settings.Default.Theme;
            theme = String.IsNullOrWhiteSpace(theme) ? "Light" : theme;
            var uri = new Uri("pack://siteoforigin:,,,/Themes/" + theme + ".xaml", UriKind.RelativeOrAbsolute);
            ResourceDictionary dict = new ResourceDictionary() { Source = uri };
            // Neues Theme hinzufügen
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        private void OnBeenden(object sender, EventArgs e) {
            Application.Current.Shutdown();
        }

        private void OnSucheSubmitted(object sender, SucheEventArgs e) {
            AbfahrtenViewModel vm = AbfahrtenViewModelFactory.GetInstance(e.HaltestelleSelected);
            _registerEvents(vm);
            vm.ConnectionLost += OnConnectionLost;
            Scheduler.ScheduleInstant(vm);
        }

        private void OnConnectionLost(object sender, EventArgs e) {
            _switchToHome();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Objekte verwerfen, bevor Bereich verloren geht")]
        private void OnInfo(object sender, EventArgs e) {
            AbstractViewModel vm = new InfoViewModel();
            _registerEvents(vm);
            Scheduler.ScheduleInstant(vm);
        }

        private void OnErrorRaised(object sender, ErrorEventArgs e) {
            ErrorViewModel vm = e.Error;
            if (vm != null) {
                try {
                    _errors.AddOrUpdate((AbstractViewModel)sender, vm, (key, oldValue) => vm);
                } catch (ArgumentException ex) {
                    //Exception bereits vorhanden, wird wohl schon bearbeitet!
                }
                RaisePropertyChangedEvent("Error");
            }
        }

        private void OnErrorCleared(object sender, EventArgs e) {
            ErrorViewModel evm;
            _errors.TryRemove((AbstractViewModel)sender, out evm);
            RaisePropertyChangedEvent("Error");
        }

        private void _registerEvents(AbstractViewModel vm) {
            vm.ErrorRaised += OnErrorRaised;
            vm.ErrorCleared += OnErrorCleared;
        }

        private void OnInitialized(object sender, EventArgs e) {
            _initializeView();
        }

        #region ButtonMap
        public ICommand ButtonMapCommand {
            get { return new DelegateCommand(_switchToMap); }
        }
        private void _switchToMap() {
            Scheduler.ScheduleInstant(Map);
        }
        #endregion

        #region ButtonHome
        public ICommand ButtonHomeCommand {
            get { return new DelegateCommand(_switchToHome); }
        }
        private void _switchToHome() {
            Scheduler.ScheduleInstant(Hauptfenster);
        }
        #endregion
    }

}
