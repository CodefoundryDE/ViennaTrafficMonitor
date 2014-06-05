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

        private IDictionary<AbstractViewModel, ErrorViewModel> _errors;
        public ErrorViewModel Error {
            get { return _errors.ElementAtOrDefault(0).Value; }
        }


        public Scheduler<AbstractViewModel> Scheduler { get; private set; }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Objekte verwerfen, bevor Bereich verloren geht")]
        public MainViewModel() {
            _errors = new Dictionary<AbstractViewModel, ErrorViewModel>();

            Hauptfenster = HauptfensterViewModelFactory.Instance;
            _initializeMapper();
            _registerEvents(Hauptfenster);
            _initializeView();

        }

        private void _initializeMapper() {
            try {
                IHaltestellenMapper hm = HaltestellenMapperFactory.Instance;
                ISteigMapper sm = SteigMapperFactory.Instance;
                ILinienMapper lm = LinienMapperFactory.Instance;
            } catch (InvalidOperationException ex) {
                Task<EErrorResult> task = _handleParsingError(ex);
                //task.Start();
                task.Wait();
                EErrorResult result = task.Result;
                switch (result) {
                    case EErrorResult.Retry: {
                            _initializeMapper();
                            break;
                        }
                    default: {
                            OnBeenden(this, new EventArgs());
                            break;
                        }
                }
            }
        }

        private async Task<EErrorResult> _handleParsingError(InvalidOperationException ex) {
            EErrorResult result = await RaiseError("Parsing-Fehler", ex.Message, EErrorButtons.RetryCancel, ex);

            return result;
        }

        private void _initializeView() {
            Einstellungen = new EinstellungenViewModel();
            Einstellungen.Beenden += OnBeenden;
            Einstellungen.Info += OnInfo;
            _registerEvents(Einstellungen);

            Suche = new SucheViewModel();
            Suche.SucheSubmitted += OnSucheSubmitted;
            _registerEvents(Suche);

            Map = MapViewModelFactory.Instance;
            Map.HaltestelleSelected += OnSucheSubmitted;
            _registerEvents(Map);

            Scheduler = new Scheduler<AbstractViewModel>();
            Scheduler.AktuellChanged += OnSchedulerAktuellChanged;
            Scheduler.ScheduleInstant(Hauptfenster);
        }

        private void OnSchedulerAktuellChanged(object Sender, EventArgs e) {
            RaisePropertyChangedEvent("Scheduler");
        }

        private void _loadTheme() {
            string theme = Properties.Settings.Default.Theme.Trim();
            theme = theme.Equals("") ? "Light" : theme;
            var uri = new Uri("pack://siteoforigin:,,,/Themes/" + theme + ".xaml", UriKind.RelativeOrAbsolute);
            ResourceDictionary dict = new ResourceDictionary() { Source = uri };
            // Neues Theme hinzufügen
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        private void OnBeenden(object sender, EventArgs e) {
            Application.Current.Shutdown();
        }

        private void OnSucheSubmitted(object sender, SucheEventArgs e) {
            AbstractViewModel vm = AbfahrtenViewModelFactory.GetInstance(e.HaltestelleSelected);
            _registerEvents(vm);
            Scheduler.ScheduleInstant(vm);
        }

        private void OnInfo(object sender, EventArgs e) {
            AbstractViewModel vm = new InfoViewModel();
            _registerEvents(vm);
            Scheduler.ScheduleInstant(vm);
        }

        private void OnErrorRaised(object sender, ErrorEventArgs e) {
            ErrorViewModel vm = e.Error;
            if (vm != null) {
                _errors.Add((AbstractViewModel)sender, vm);
                RaisePropertyChangedEvent("Error");
            }
        }

        private void OnErrorCleared(object sender, EventArgs e) {
            _errors.Remove((AbstractViewModel)sender);
            RaisePropertyChangedEvent("Error");
        }

        private void _registerEvents(AbstractViewModel vm) {
            vm.ErrorRaised += OnErrorRaised;
            vm.ErrorCleared += OnErrorCleared;
        }

        #region ButtonMap
        public ICommand ButtonMapCommand {
            get { return new DelegateCommand(_switchToMap); }
        }
        private void _switchToMap() {
            Scheduler.ScheduleInstant(Map);
        }
        #endregion

    }

}
