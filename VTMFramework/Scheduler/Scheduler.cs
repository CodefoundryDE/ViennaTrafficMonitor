using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using VtmFramework.ViewModel;

namespace VtmFramework.Scheduler {

    public class Scheduler<T> : IDisposable where T : AbstractViewModel {

        private bool _disposed = false;

        private ConcurrentQueue<T> _queue;
        private Timer _timer;

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

        /// <summary>
        /// Zeigt ein Element sofort an.
        /// </summary>
        /// <param name="element"></param>
        public void ScheduleInstant(T element) {
            _Pause();
            Aktuell = element;
            _RaiseAktuellChangedEvent();
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
            T temp;
            // Es darf nur geswitcht werden wenn aktuell kein Error vorhanden ist
            if (Aktuell == null || Aktuell.Error == null) {
                if (_queue.TryDequeue(out temp)) {
                    Aktuell = temp;
                    _RaiseAktuellChangedEvent();
                    _queue.Enqueue(temp);
                }
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
