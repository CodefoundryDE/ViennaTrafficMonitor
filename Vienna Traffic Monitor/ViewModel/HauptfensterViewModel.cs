using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtmFramework.Error;
using VtmFramework.ViewModel;

namespace ViennaTrafficMonitor.ViewModel {

    public class HauptfensterViewModel : AbstractViewModel {

        public HauptfensterViewModel() {
            RaiseError("Hallo Welt!", "Ganz strenge Fehlermeldung!", EErrorButtons.OkCancel);
        }

    }

}
