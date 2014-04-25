using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtmFramework.Error;
using VtmFramework.Logging;

namespace VtmFramework.ViewModel {

    public static class ErrorViewModelFactory {

        private static IVTMLogger _logger = new VTMLogger();

        [Obsolete ("Use getInsance with exception for logging")]
        public static ErrorViewModel GetInstance(string title, string message, EErrorButtons buttonSet, IObserver<EErrorResult> observer) {
            ErrorViewModel evm = new ErrorViewModel();
            evm.Title = title;
            evm.Message = message;
            evm.ButtonSet = buttonSet;
            evm.Subscribe(observer);

            return evm;
        }

        public static ErrorViewModel GetInstance (Exception ex, string title, string message, EErrorButtons buttonSet, IObserver<EErrorResult> observer){
            ErrorViewModel evm = new ErrorViewModel(title, message, buttonSet, ex, _logger);
            evm.Subscribe(observer);

            return evm;
    }



    }

}
