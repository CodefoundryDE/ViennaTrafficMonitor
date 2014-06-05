using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using VtmFramework.Logging;
using VtmFramework.ViewModel;
using System.Linq;
using System.Net.Http;
using System.IO;
using System.Media;
using System.Windows;

namespace VtmFramework.Scheduler {

    public sealed class Scheduler<T> : IDisposable where T : AbstractViewModel {

        private const int DEFAULT_INTERVAL = 3000;

        private ConcurrentQueue<T> _queue;
        private Timer _timer;

        /// <summary>
        /// Default-Konstruktor
        /// </summary>
        public Scheduler() {
            this._queue = new ConcurrentQueue<T>();
            Interval = DEFAULT_INTERVAL;
            _timer = new Timer(_Tick, null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Verzögerung, die der Scheduler zwischen den Auswechslungen abwartet.
        /// </summary>
        public int Interval { get; set; }

        #region Aktuelles Element im Scheduler
        /// <summary>
        /// Das aktuelle Element.
        /// </summary>
        private T _aktuell;
        public T Aktuell {
            get { return _aktuell; }
            private set {
                _aktuell = value;
                _RaiseAktuellChangedEvent();
            }
        }

        /// <summary>
        /// Dieses Event wird ausgelöst, wenn sich der Aktuelle Inhalt des Schedulers ändert.
        /// </summary>
        public event PropertyChangedEventHandler AktuellChanged;

        /// <summary>
        /// Löst das AktuellChanged-Event aus.
        /// </summary>
        private void _RaiseAktuellChangedEvent() {
            var handler = AktuellChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs("Aktuell"));
        }
        #endregion

        #region Methoden zur Scheduler-Steuerung
        /// <summary>
        /// Reiht ein Element in die Warteschlange ein, so dass es periodisch angezeigt wird.
        /// </summary>
        /// <param name="element"></param>
        public void Schedule(T element) {
            _queue.Enqueue(element);
        }

        /// <summary>
        /// Zeigt ein Element sofort an.
        /// </summary>
        /// <param name="element"></param>
        public void ScheduleInstant(T element) {
            _Stop();
            Aktuell = element;
        }

        /// <summary>
        /// Startet den Scheduler.
        /// </summary>
        public void Start() {
            _timer.Change(0, Interval);
        }
        #endregion

        /// <summary>
        /// Pausiert den Scheduler.
        /// </summary>
        private void _Stop() {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            _queue = new ConcurrentQueue<T>();
        }

        /// <summary>
        /// Führt eine Runde des Schedulers aus.
        /// Das erste Element der Warteschlange wird aktuell und anschließend wieder hinten eingereiht.
        /// </summary>
        private void _Tick(object state) {
            T temp;
            if (_queue.TryDequeue(out temp)) {
                Aktuell = temp;
                _queue.Enqueue(temp);
            }
        }

        #region Methoden für den Garbage-Collector
        private bool _disposed = false;
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
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
