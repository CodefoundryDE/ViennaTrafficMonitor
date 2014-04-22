﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtmFramework.Error;

namespace VtmFramework.ViewModel {

    public static class ErrorViewModelFactory {

        public static ErrorViewModel GetInstance(string title, string message, EErrorButtons buttonSet, IObserver<EErrorResult> observer) {
            ErrorViewModel evm = new ErrorViewModel();
            evm.Title = title;
            evm.Message = message;
            evm.ButtonSet = buttonSet;
            evm.Subscribe(observer);

            return evm;
        }

    }

}