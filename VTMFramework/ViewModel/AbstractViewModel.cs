using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VtmFramework.Error;

namespace VtmFramework.ViewModel {

    public abstract class AbstractViewModel : IObserver<EErrorResult>, IDisposable, INotifyPropertyChanged {

        private bool _disposed;

        public event EventHandler<ErrorEventArgs> ErrorRaised;
        public event EventHandler ErrorCleared;
        private readonly AutoResetEvent _errorCompleted;
        private EErrorResult _errorResult;

        //public ErrorViewModel Error { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected AbstractViewModel() {
            _errorCompleted = new AutoResetEvent(false);
        }

        private async Task<EErrorResult> _raiseError() {
            await Task.Run(() => _errorCompleted.WaitOne());
            return _errorResult;
        }

        protected async Task<EErrorResult> RaiseError(string title, string message, EErrorButtons buttonSet) {
            var error = ErrorViewModelFactory.GetInstance(title, message, buttonSet, this);
            RaiseErrorEvent(error);
            var result = await _raiseError();
            ClearErrorEvent();
            return result;
        }

        protected async Task<EErrorResult> RaiseError(string title, string message, EErrorButtons buttonSet, Exception ex) {
            var error = ErrorViewModelFactory.GetInstance(title, message, buttonSet, this, ex);
            RaiseErrorEvent(error);
            var result = await _raiseError();
            ClearErrorEvent();
            return result;
        }

        private void RaiseErrorEvent(ErrorViewModel error) {
            var handler = ErrorRaised;
            if (handler != null) {
                handler(this, new ErrorEventArgs(error));
            }
        }

        private void ClearErrorEvent() {
            var handler = ErrorCleared;
            if (handler != null) {
                handler(this, new EventArgs());
            }
        }

        public void OnScheduled(object sender, EventArgs e) {
            Init();
        }

        protected virtual void Init() {

        }

        protected void RaisePropertyChangedEvent(string propertyName) {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
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
            if (_disposed) return;
            if (disposing) {
                _errorCompleted.Set();
                _errorCompleted.Dispose();
            }
            _disposed = true;
        }

        ~AbstractViewModel() {
            Dispose(false);
        }
        #endregion
    }

}
