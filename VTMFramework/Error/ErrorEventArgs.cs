using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtmFramework.ViewModel;

namespace VtmFramework.Error {

    public class ErrorEventArgs : EventArgs {

        public ErrorEventArgs(ErrorViewModel error) {
            this.Error = error;
        }

        public ErrorViewModel Error { get; set; }

    }

}
