using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtmFramework.ViewModel;

namespace ViennaTrafficMonitor.ViewModel {

    public class MainViewModel : AbstractViewModel {

        /* Man könnte hier über ein Strategie-Muster verschiedene Animationen zum Bildschirm-Wechseln einhängen */

        private AbstractViewModel _activeViewModel;

        public AbstractViewModel ActiveViewModel {
            get { return _activeViewModel; }
            set { _activeViewModel = value; RaisePropertyChangedEvent("ActiveViewModel"); }
        }
        

    }

}
