﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VtmFramework.Error;

namespace VtmFramework.ViewModel {

    public abstract class AbstractViewModel : IViewModel, IObserver<EErrorResult>, IDisposable {

        private bool _disposed = false;
        private object syncRoot = new object();

        private AutoResetEvent _errorCompleted;
        private EErrorResult _errorResult;

        public ErrorViewModel Error { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected AbstractViewModel() {
            _canSwitch = true;
            _errorCompleted = new AutoResetEvent(false);
        }

        private async Task<EErrorResult> _raiseError() {
            RaisePropertyChangedEvent("Error");
            await Task.Run(() => _errorCompleted.WaitOne());
            this.Error = null;
            RaisePropertyChangedEvent("Error");
            return _errorResult;
        }

        protected async Task<EErrorResult> RaiseError(string title, string message, EErrorButtons buttonSet) {
            this.Error = ErrorViewModelFactory.GetInstance(title, message, buttonSet, this);
            return await _raiseError();
        }

        protected async Task<EErrorResult> RaiseError(string title, string message, EErrorButtons buttonSet, Exception ex) {
            this.Error = ErrorViewModelFactory.GetInstance(title, message, buttonSet, this, ex);
            return await _raiseError();
        }

        protected void RaisePropertyChangedEvent(string propertyName) {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Zeigt an, ob der Scheduler das ViewModel wechseln darf.
        /// </summary>
        /// <returns></returns>
        private bool _canSwitch;
        public bool CanSwitch {
            get {
                lock (syncRoot) {
                    return (Error == null) && _canSwitch;
                }
            }
            set {
                lock (syncRoot) {
                    _canSwitch = value;
                }
            }
        }

        #region Observer
        public void OnCompleted() {
            _errorCompleted.Set();
        }

        public void OnError(Exception error) {
            //throw new NotImplementedException();
        }

        public void OnNext(EErrorResult value) {
            _errorResult = value;
        }
        #endregion

        #region Methoden für den Garbage-Collector
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    _errorCompleted.Set();
                    _errorCompleted.Dispose();
                }
                _disposed = true;
            }
        }

        ~AbstractViewModel() {
            Dispose(false);
        }
        #endregion
    }

}
