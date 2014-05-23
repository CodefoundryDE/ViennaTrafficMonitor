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
using ViennaTrafficMonitor.Events;

namespace ViennaTrafficMonitor.ViewModel {

    public class SucheViewModel : AbstractViewModel {

        public delegate void SucheSubmittedEventHandler(SucheEventArgs e);
        public event SucheSubmittedEventHandler SucheSubmitted;

        private string _searchText;
        public string SearchText {
            get { return _searchText; }
            set {
                _searchText = value;
                RaisePropertyChangedEvent("SearchText");
                _textChangedAsync();
            }
        }

        private ICollection<IHaltestelle> _matches;
        public ICollection<IHaltestelle> Matches {
            get { return _matches; }
            private set {
                _matches = value; RaisePropertyChangedEvent("Matches");
            }
        }

        public IHaltestelle SelectedItem { get; set; }

        private IHaltestellenMapper _mapper;

        public SucheViewModel() {
            _mapper = HaltestellenMapperFactory.Instance;
        }

        private async void _textChangedAsync() {
            await Task.Run(() => {
                Matches = (from haltestelle in _mapper.FindByName(SearchText)
                           select haltestelle).ToList();
            });
        }

        public ICommand SubmitCommand { get { return new DelegateCommand(_submit); } }
        public void _submit() {
            if (SelectedItem != null) {
                OnSucheSubmitted(SelectedItem.Id);
                Matches.Clear();
                SearchText = "";
            }
        }

        private void OnSucheSubmitted(int haltestelleSelected) {
            SucheSubmittedEventHandler handler = SucheSubmitted;
            if (handler != null) {
                handler(new SucheEventArgs(haltestelleSelected));
            }
        }

    }

}
