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

        protected async override void Init() {
            bool success = await _testMapperInitialization();
            if (success) {
                RaiseInitializedEvent();
            }
        }

        #region TestMapper
        private async Task<bool> _testMapperInitialization() {
            Task<EErrorResult> task = null;
            try {
                var haltestellenMapper = HaltestellenMapperFactory.Instance;
                var steigMapper = SteigMapperFactory.Instance;
                var linienMapper = LinienMapperFactory.Instance;
            } catch (InvalidOperationException ex) {
                task = _handleParsingError(ex);                
            }
            if (task == null) return true;
            var result = await task;
            switch (result) {
                case EErrorResult.Retry: {
                    await _testMapperInitialization();
                    return true;
                }
                default: {
                    RaiseBeendenEvent();
                    return false;
                }
            }
        }
        private Task<EErrorResult> _handleParsingError(InvalidOperationException ex) {
            //EErrorResult result = await RaiseError("Parsing-Fehler", ex.Message, EErrorButtons.RetryCancel, ex);
            //return result;
            return RaiseError("Parsing-Fehler", ex.Message, EErrorButtons.RetryCancel, ex);
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
