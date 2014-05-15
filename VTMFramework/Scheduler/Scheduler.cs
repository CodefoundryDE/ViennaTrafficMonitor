using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using VtmFramework.Logging;
using VtmFramework.ViewModel;
using System.Linq;

namespace VtmFramework.Scheduler {

    public class Scheduler<T> : IDisposable where T : AbstractViewModel {

        private bool _disposed = false;

        private ConcurrentQueue<T> _queue;
        private ConcurrentDictionary<string, T> _dictionary;
        private Timer _timer;
        private int _counter;


        /// <summary>
        /// Verzögerung, die der Scheduler zwischen den Auswechslungen abwartet.
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// Das aktuelle Element.
        /// </summary>
        public T Aktuell { get; private set; }

        /// <summary>
        /// Dieses Event wird ausgelöst, wenn sich der Aktuelle Inhalt des Schedulers ändert.
        /// </summary>
        public event PropertyChangedEventHandler AktuellChanged;

        public Scheduler() {
            this._queue = new ConcurrentQueue<T>();
            this._dictionary = new ConcurrentDictionary<string, T>();
            _counter = 0;
            Delay = 1500;
            _timer = new Timer(_Tick, null, Timeout.Infinite, Delay);
        }

        /// <summary>
        /// Reiht ein Element in die Warteschlange ein, so dass es periodisch angezeigt wird.
        /// </summary>
        /// <param name="element"></param>
        public void Schedule(T element) {
            _queue.Enqueue(element);
        }

        public void Schedule(string viewName, T element) {
            _dictionary.AddOrUpdate(viewName, element, (key, oldValue) => { return element; });
        }

        /// <summary>
        /// Zeigt ein Element sofort an.
        /// </summary>
        /// <param name="element"></param>
        public void ScheduleInstant(T element) {
            _Pause();
            Aktuell = element;
            _RaiseAktuellChangedEvent();
        }

        public void ScheduleInstant(string viewName) {
            T newView;
            if (_dictionary.TryGetValue(viewName, out newView)) {
                _Pause();
                Aktuell = newView;
                _RaiseAktuellChangedEvent();
            } else {
                IVtmLogger logger = VtmLoggerFactory.GetInstance();
                logger.Error("View \"" + viewName + "\"beim Versuch diese anzuzeigen nicht im ViewDictionary gefunden!");
            }
        }

        public void ScheduleInstant(string viewName, T element) {
            Schedule(viewName, element);
            ScheduleInstant(viewName);
        }

        private bool _getViewByIndex(int index, out T newView) {
            try {
                _dictionary.TryGetValue(_dictionary.Keys.ElementAt(index), out newView);
            } catch (Exception ex) {
                if (ex is ArgumentOutOfRangeException) { }
                IVtmLogger logger = VtmLoggerFactory.GetInstance();
                logger.Error("View am Index \"" + index + "\" nicht gefunden" + ((ArgumentOutOfRangeException)ex).StackTrace);
                newView = null;
                return false;
            }
            return true;
        }

        private T _getNextView() {
            T newView;
            if (++_counter >= _dictionary.Count()) {
                _counter = 0;
            }
            _getViewByIndex(_counter, out newView);
            if (newView == null) {
                return Aktuell;
            }
            return newView;

        }

        /// <summary>
        /// Startet den Scheduler.
        /// </summary>
        public void Start() {
            _timer.Change(0, Delay);
        }

        /// <summary>
        /// Pausiert den Scheduler.
        /// </summary>
        private void _Pause() {
            _timer.Change(Timeout.Infinite, Delay);
        }

        /// <summary>
        /// Führt eine Runde des Schedulers aus.
        /// Das erste Element der Warteschlange wird aktuell und anschließend wieder hinten eingereiht.
        /// </summary>
        private void _Tick(object state) {
            if (Aktuell == null || Aktuell.CanSwitch()) {
                Aktuell = _getNextView();
                _RaiseAktuellChangedEvent();
            }
        }

        /// <summary>
        /// Löst das AktuellChanged-Event aus.
        /// </summary>
        private void _RaiseAktuellChangedEvent() {
            var handler = AktuellChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs("Aktuell"));
        }

        #region Methoden für den Garbage-Collector
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    _timer.Dispose();
                }
                _disposed = true;
            }
        }

        ~Scheduler() {
            Dispose(false);
        }
        #endregion

    }

}
