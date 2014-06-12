using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VtmFramework.Command;
using VtmFramework.Error;
using VtmFramework.Logging;

namespace VtmFramework.ViewModel {

    public class ErrorViewModel : INotifyPropertyChanged, IObservable<EErrorResult> {

        private EErrorButtons _buttonSet;

        #region ButtonProperties
        public bool ButtonOk {
            get {
                return ((_buttonSet == EErrorButtons.Ok) || (_buttonSet == EErrorButtons.OkCancel)) ? true : false;
            }
        }

        public bool ButtonNo {
            get {
                return (_buttonSet == EErrorButtons.YesNo) ? true : false;
            }
        }

        public bool ButtonCancel {
            get {
                return ((_buttonSet == EErrorButtons.OkCancel) || (_buttonSet == EErrorButtons.RetryCancel)) ? true : false;
            }
        }

        public bool ButtonRetry {
            get {
                return _buttonSet == EErrorButtons.RetryCancel ? true : false;
            }
        }

        public bool ButtonYes {
            get {
                return _buttonSet == EErrorButtons.YesNo ? true : false;
            }
        }
        #endregion

        private class Unsubscriber : IDisposable {
            public void Dispose() {
                throw new NotSupportedException("Dispose on ErrorViewHandler not Supported!");
            }
        }

        public ErrorViewModel()
            : base() {
            Visible = false;
            _buttonSet = EErrorButtons.Empty;
        }

        public ErrorViewModel(EErrorButtons buttonSet)
            : base() {
            Visible = true;
            _buttonSet = buttonSet;
        }

        public ErrorViewModel(Exception ex, IVtmLogger logger, EErrorButtons buttonSet)
            : this(buttonSet) {
            if (logger != null) logger.Error(ex);
        }

        private IObserver<EErrorResult> _observer;

        public string Title { get; set; }
        private string _message;
        public string Message {
            get { return _message; }
            set { _message = value; RaisePropertyChangedEvent("Message"); }
        }
        private bool _visible;
        public bool Visible {
            get { return _visible; }
            set { _visible = value; RaisePropertyChangedEvent("Visible"); }
        }
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

        #region ButtonOk
        public ICommand ButtonOkCommand {
            get { return new DelegateCommand(_ButtonOk); }
        }

        private void _ButtonOk() {
            _observer.OnNext(EErrorResult.Ok);
            _observer.OnCompleted();
            Visible = false;
        }

        #endregion

        #region ButtonCancel
        public ICommand ButtonCancelCommand {
            get { return new DelegateCommand(_ButtonCancel); }
        }

        private void _ButtonCancel() {
            _observer.OnNext(EErrorResult.Cancel);
            _observer.OnCompleted();
            Visible = false;
        }
        #endregion

        #region ButtonYes
        public ICommand ButtonYesCommand {
            get { return new DelegateCommand(_ButtonYes); }
        }

        private void _ButtonYes() {
            _observer.OnNext(EErrorResult.Yes);
            _observer.OnCompleted();
            Visible = false;
        }
        #endregion

        #region ButtonNo
        public ICommand ButtonNoCommand {
            get { return new DelegateCommand(_ButtonNo); }
        }

        private void _ButtonNo() {
            _observer.OnNext(EErrorResult.No);
            _observer.OnCompleted();
            Visible = false;
        }
        #endregion

        #region ButtonRetry
        public ICommand ButtonRetryCommand {
            get { return new DelegateCommand(_ButtonRetry); }
        }

        private void _ButtonRetry() {
            _observer.OnNext(EErrorResult.Retry);
            _observer.OnCompleted();
            Visible = false;
        }
        #endregion
    }
}
