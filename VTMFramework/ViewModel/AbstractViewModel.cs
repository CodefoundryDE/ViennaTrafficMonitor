using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtmFramework.Error;

namespace VtmFramework.ViewModel {

    public abstract class AbstractViewModel : IViewModel, IObserver<EErrorResult> {

        public ErrorViewModel Error { get; set; }
        protected void RaiseError(string title, string message, EErrorButtons buttonSet) {
            this.Error = ErrorViewModelFactory.getInstance(title, message, EErrorButtons.OkCancel, this);
        }

        public event PropertyChangedEventHandler PropertyChanged;
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
            throw new NotImplementedException();
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnNext(EErrorResult value) {
            throw new NotImplementedException();
        }
    }

}
