using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VtmFramework.Command;
using ViennaTrafficMonitor.Model;
using VtmFramework.ViewModel;
using ViennaTrafficMonitor.Mapper;
using System.Collections.Generic;
using System.Threading;

namespace ViennaTrafficMonitor.ViewModel {

    public class SucheViewModel : AbstractViewModel {

        private string _searchText;
        public string SearchText {
            get { return _searchText; }
            set {
                _searchText = value;
                RaisePropertyChangedEvent("SearchText");
                _buttonSearchClick();
            }
        }

        private List<string> _matches;
        public List<string> Matches {
            get { return _matches; }
            private set {
                _matches = value; RaisePropertyChangedEvent("Matches");
            }
        }

        private IHaltestellenMapper _mapper;

        public SucheViewModel() {
            _mapper = HaltestellenMapperFactory.Instance;
        }

        public ICommand ButtonSearchClickCommand { get { return new AwaitableDelegateCommand(_buttonSearchClickAsync); } }

        private async Task _buttonSearchClickAsync() {
            await Task.Run(() => {
                Matches = (from haltestelle in _mapper.FindByName(SearchText)
                           select haltestelle.Name).ToList(); ;

            });
        }

        private async void _buttonSearchClick() {
            await _buttonSearchClickAsync();
        }

    }

}
