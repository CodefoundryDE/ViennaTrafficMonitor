using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViennaTrafficMonitor.Mapper;
using ViennaTrafficMonitor.View;
using VtmFramework.Error;
using VtmFramework.ViewModel;

namespace ViennaTrafficMonitor.ViewModel {
    public class InitializationViewModel : AbstractViewModel {

        public event EventHandler Beenden;
        public event EventHandler Initialized;

        public InitializationViewModel() {

        }

        protected override void Init() {
            _testMapperInitialization();
            RaiseInitializedEvent();
        }

        #region TestMapper
        private void _testMapperInitialization() {
            try {
                IHaltestellenMapper hm = HaltestellenMapperFactory.Instance;
                ISteigMapper sm = SteigMapperFactory.Instance;
                ILinienMapper lm = LinienMapperFactory.Instance;
            } catch (InvalidOperationException ex) {
                Task<EErrorResult> task = _handleParsingError(ex);
                task.Wait();
                EErrorResult result = task.Result;
                switch (result) {
                    case EErrorResult.Retry: {
                            //_testMapperInitialization();
                        MessageBox.Show("Habe Retry als Antwort erhalten!!!!");
                            break;
                        }
                    default: {
                            RaiseBeendenEvent();
                            break;
                        }
                }
            }
        }
        private async Task<EErrorResult> _handleParsingError(InvalidOperationException ex) {
            EErrorResult result = await RaiseError("Parsing-Fehler", ex.Message, EErrorButtons.RetryCancel, ex);
            return result;
        }
        #endregion


        #region Beenden
        private void RaiseBeendenEvent() {
            EventHandler handler = Beenden;
            if (handler != null) {
                handler(this, new EventArgs());
            }
        }
        #endregion

        #region InitializationFinished
        private void RaiseInitializedEvent() {
            EventHandler handler = Initialized;
            if (handler != null) {
                handler(this, new EventArgs());
            }
        }
        #endregion

    }
}
