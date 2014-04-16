using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtmFramework.Error;

namespace VtmFramework.ViewModel {

    public class ErrorViewModel : IViewModel, IObservable<EErrorResult> {
        private IObserver<EErrorResult> _observer;
        public event PropertyChangedEventHandler PropertyChanged;
        public string Message { get; set; }
        public EErrorButtons ButtonSet { get; set; }
        protected void RaisePropertyChangedEvent(string propertyName) {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }




        /// <summary>
        /// Funktion um den Aufrufer des Errorhandlings 
        /// als Observer auf dem ErrorViewModel zu registrieren.
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        public IDisposable Subscribe(IObserver<EErrorResult> observer) {
            this._observer = observer;
            return new Unsubscriber();
        }
    }

    private class Unsubscriber : IDisposable {
        public void Dispose() {
            throw new NotSupportedException("Dispose on ErrorViewHandler not Supported!");
        }
    }
}
