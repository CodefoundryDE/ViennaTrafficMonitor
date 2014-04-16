using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtmFramework.ViewModel;

namespace ViennaTrafficMonitor.ViewModel {

    public class HauptfensterViewModel : AbstractViewModel {

        public HauptfensterViewModel() {
            this.Error = ErrorViewModelFactory.getInstance("Hallo Welt", "Ganz strenge Fehlermeldung", VtmFramework.Error.EErrorButtons.OkCancel, this);
            RaisePropertyChangedEvent("Error");
        }

    }

}
