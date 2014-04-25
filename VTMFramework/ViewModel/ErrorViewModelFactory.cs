using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtmFramework.Error;

namespace VtmFramework.ViewModel {

    public static class ErrorViewModelFactory {

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
            ErrorViewModel evm = new ErrorViewModel(ex, title, message, buttonSet);
            evm.Subscribe(observer);

            return evm;
    }



    }

}
