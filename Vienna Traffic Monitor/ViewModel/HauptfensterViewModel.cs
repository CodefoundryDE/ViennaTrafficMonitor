using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ViennaTrafficMonitor.Mapper;
using ViennaTrafficMonitor.Model;
using VtmFramework.Command;
using VtmFramework.Error;
using VtmFramework.ViewModel;

namespace ViennaTrafficMonitor.ViewModel {

    public class HauptfensterViewModel : AbstractViewModel {

        private IHaltestellenMapper _haltestellenMapper;
        private ICollection<IHaltestelle> _haltestellen6;
        private ICollection<IHaltestelle> _haltestellen7;
        private Random _randomNumber;

        private IList<String> _flap;
        public IList<String> Flap {
            get { return _flap; }
            private set {
                _flap = value;
                RaisePropertyChangedEvent("Flap");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        Timer FlapTimer;

        public HauptfensterViewModel(IHaltestellenMapper haltestellenMapper)
            : base() {
            if (haltestellenMapper != null) {
                _randomNumber = new Random();
                _haltestellenMapper = haltestellenMapper;
                _haltestellen6 = _haltestellenMapper.GetByNameLength(6);
                _haltestellen7 = _haltestellenMapper.GetByNameLength(7);
                Flap = new List<String> { "VIENNA", "TRAFFIC", "MONITOR" };
                FlapTimer = new Timer(_flapTick, null, 5000, 5000);
            }
        }

        private void _flapTick(object state) {
            Flap[0] = _haltestellen6.ElementAt(_randomNumber.Next(_haltestellen6.Count)).Name;
            Flap[1] = _haltestellen7.ElementAt(_randomNumber.Next(_haltestellen7.Count)).Name;
            Flap[2] = _haltestellen7.ElementAt(_randomNumber.Next(_haltestellen7.Count)).Name;
            RaisePropertyChangedEvent("Flap");
        }

    }

}
