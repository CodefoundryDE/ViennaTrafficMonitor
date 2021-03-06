﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtmFramework.Error;
using VtmFramework.Logging;

namespace VtmFramework.ViewModel {

    public static class ErrorViewModelFactory {

        private static IVtmLogger _logger = VtmLoggerFactory.GetInstance();

        public static ErrorViewModel GetInstance(string title, string message, EErrorButtons buttonSet, IObserver<EErrorResult> observer) {
            ErrorViewModel evm = new ErrorViewModel(buttonSet);
            evm.Title = title;
            evm.Message = message;
            evm.ButtonSet = buttonSet;
            evm.Subscribe(observer);

            return evm;
        }

        public static ErrorViewModel GetInstance(string title, string message, EErrorButtons buttonSet, IObserver<EErrorResult> observer, Exception ex) {
            ErrorViewModel evm = new ErrorViewModel(ex, _logger, buttonSet);
            evm.Title = title;
            evm.Message = message;
            evm.ButtonSet = buttonSet;
            evm.Subscribe(observer);

            return evm;
        }



    }

}
