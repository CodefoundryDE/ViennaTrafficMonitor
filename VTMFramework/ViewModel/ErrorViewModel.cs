using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VtmFramework.Command;
using VtmFramework.Error;

namespace VtmFramework.ViewModel {

    public class ErrorViewModel : IViewModel, IObservable<EErrorResult> {

        private class Unsubscriber : IDisposable {
            public void Dispose() {
                throw new NotSupportedException("Dispose on ErrorViewHandler not Supported!");
            }
        }

        public ErrorViewModel() : base() {
            Visible = Visibility.Visible;
        }

        private IObserver<EErrorResult> _observer;

        public string Title { get; set; }
        public string Message { get; set; }
        public Visibility Visible { get; private set; }
        public EErrorButtons ButtonSet { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
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

        public ICommand ButtonOkCommand {
            get { return new DelegateCommand(_ButtonOk); }
        }

        private void _ButtonOk() {
            _observer.OnNext(EErrorResult.Ok);
            _observer.OnCompleted();
        }
    }
}
