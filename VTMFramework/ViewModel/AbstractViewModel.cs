using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VtmFramework.Error;

namespace VtmFramework.ViewModel {

    public abstract class AbstractViewModel : IViewModel, IObserver<EErrorResult> {

        private AutoResetEvent _errorCompleted;
        private EErrorResult _errorResult;

        public ErrorViewModel Error { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected AbstractViewModel() {
            _errorCompleted = new AutoResetEvent(false);
        }

        [Obsolete("Use RaiseError including an execption for proper treatment and logging")]
        protected async Task<EErrorResult> RaiseError(string title, string message, EErrorButtons buttonSet) {
            this.Error = ErrorViewModelFactory.GetInstance(title, message, buttonSet, this);
            RaisePropertyChangedEvent("Error");
            await Task.Run(() => _errorCompleted.WaitOne());
            this.Error = null;
            RaisePropertyChangedEvent("Error");
            return _errorResult;
        }

        protected async Task<EErrorResult> RaiseError(Exception ex, string title, string message, EErrorButtons buttonSet)
        {
            this.Error = ErrorViewModelFactory.GetInstance(ex, title, message, buttonSet, this);
            RaisePropertyChangedEvent("Error");
            await Task.Run(() => _errorCompleted.WaitOne());
            this.Error = null;
            RaisePropertyChangedEvent("Error");
            return _errorResult;
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
        public bool CanSwitch() {
            return (Error == null);
        }

        public void OnCompleted() {
            _errorCompleted.Set();
        }

        public void OnError(Exception error) {
            //throw new NotImplementedException();
        }

        public void OnNext(EErrorResult value) {
            _errorResult = value;
        }
    }

}
