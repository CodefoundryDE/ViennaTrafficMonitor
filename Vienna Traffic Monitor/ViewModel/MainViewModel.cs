﻿using System;
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
            //_loadTheme();

            Einstellungen = new EinstellungenViewModel();
            Einstellungen.Beenden += OnBeenden;
            Einstellungen.Info += OnInfo;
            Suche = new SucheViewModel();
            Suche.SucheSubmitted += _sucheSubmitted;

            Scheduler = new Scheduler<AbstractViewModel>();
            Scheduler.AktuellChanged += OnSchedulerAktuellChanged;
            Scheduler.ScheduleInstant(new HauptfensterViewModel());
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

        public EinstellungenViewModel Einstellungen { get; private set; }

        private void OnBeenden(object sender, EventArgs e) {
            Application.Current.Shutdown();
        }

        private void OnInfo(object sender, EventArgs e) {
            Scheduler.ScheduleInstant(new InfoViewModel());
        }

        private void _sucheSubmitted(object sender, SucheEventArgs e) {
            Scheduler.ScheduleInstant(AbfahrtenViewModelFactory.GetInstance(e.HaltestelleSelected));
        }


        #region ButtonMap
        public ICommand ButtonMapCommand {
            get { return new DelegateCommand(_switchToMap); }
        }
        private void _switchToMap() {
            Scheduler.ScheduleInstant(MapViewModelFactory.Instance);
        }
        #endregion

    }

}
