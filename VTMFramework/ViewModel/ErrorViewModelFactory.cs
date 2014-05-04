using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtmFramework.Error;
using VtmFramework.Logging;

namespace VtmFramework.ViewModel {

    public static class ErrorViewModelFactory {

        private static IVTMLogger _logger = VTMLoggerFactory.getInstance();

        public static ErrorViewModel GetInstance(string title, string message, EErrorButtons buttonSet, IObserver<EErrorResult> observer) {
            ErrorViewModel evm = new ErrorViewModel();
            evm.Title = title;
            evm.Message = message;
            evm.ButtonSet = buttonSet;
            evm.Subscribe(observer);

            return evm;
        }

        public static ErrorViewModel GetInstance(string title, string message, EErrorButtons buttonSet, IObserver<EErrorResult> observer, Exception ex) {
            ErrorViewModel evm = new ErrorViewModel(ex, _logger);
            evm.Title = title;
            evm.Message = message;
            evm.ButtonSet = buttonSet;
            evm.Subscribe(observer);

            return evm;
        }



    }

}
