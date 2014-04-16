using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace VtmFramework.Scheduler {

    public class Scheduler<T> : IDisposable{

        private bool _disposed = false;

        private ConcurrentQueue<T> _queue;
        private Task _contentSwitcher;
        private CancellationTokenSource _cts;

        /// <summary>
        /// Verzögerung, die der Scheduler zwischen den Auswechslungen abwartet.
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// Das aktuelle Element.
        /// </summary>
        public T Aktuell { get; set; }

        /// <summary>
        /// Dieses Event wird ausgelöst, wenn sich der Aktuelle Inhalt des Schedulers ändert.
        /// </summary>
        public event PropertyChangedEventHandler AktuellChanged;

        public Scheduler() {
            this._queue = new ConcurrentQueue<T>();
            _cts = new CancellationTokenSource();
            _contentSwitcher = new Task(_Loop, _cts.Token, TaskCreationOptions.LongRunning);
            Delay = 1500;
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
            _contentSwitcher.Start();
        }

        /// <summary>
        /// Führt eine Runde des Schedulers aus.
        /// Das erste Element der Warteschlange wird aktuell und anschließend wieder hinten eingereiht.
        /// </summary>
        private void _Loop() {
            T temp;
            while (true) {
                if (_queue.TryDequeue(out temp)) {
                    Aktuell = temp;
                    _RaiseAktuellChangedEvent();
                    _queue.Enqueue(temp);
                }
                Thread.Sleep(Delay);
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

        /// <summary>
        /// Pausiert den Scheduler.
        /// </summary>
        private void _Pause() {
            _cts.Cancel();
        }

        #region Methoden für den Garbage-Collector
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    _cts.Dispose();
                    _contentSwitcher.Dispose();
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
