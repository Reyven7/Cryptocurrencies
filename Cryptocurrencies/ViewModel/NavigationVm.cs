using Cryptocurrencies.Model;
using Cryptocurrencies.Tools;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HandyControl.Controls;
using MessageBox = System.Windows.MessageBox;

namespace Cryptocurrencies.ViewModel
{
    internal class NavigationVm : ViewModelBase
    {
        private readonly SelectedItem _selectedItem = SelectedItem.Instance;
        private readonly ObservableCollection<Cryptocurrency> _cryptocurrencies = [];

        private ObservableCollection<Cryptocurrency> _cryptocurrenciesSearched;
        private object _currentView;
        private string _searchQuery;
        private bool _isPopupOpen;
        private Cryptocurrency _selectedCryptocurrency;

        public NavigationVm()
        {
            HomeCommand = new RelayCommand(Home);
            ConverterCommand = new RelayCommand(Converter);
            SelectCryptoCommand = new RelayCommand(OnCryptoSelected);

            CurrentView = new HomeVm();

            Uri url = new("https://api.coincap.io/v2/assets/");
             _ = HttpRequest.GetCryptocurrenciesAsync(url, _cryptocurrencies);
        }

        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
                SearchCryptocurrencies(value);
            }
        }

        public Cryptocurrency SelectedCryptocurrency
        {
            get => _selectedCryptocurrency;
            set
            {
                if (_selectedCryptocurrency == value) return;

                _selectedCryptocurrency = value;
                OnPropertyChanged();
            }
        }

        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set
            {
                _isPopupOpen = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Cryptocurrency> CryptocurrenciesSearched
        {
            get => _cryptocurrenciesSearched;
            set
            {
                _cryptocurrenciesSearched = value;
                OnPropertyChanged();
            }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand ConverterCommand { get; set; }
        public ICommand SelectCryptoCommand { get; }

        private void Home(object obg) => CurrentView = new HomeVm();

        private void Converter(object obg) => CurrentView = new ConverterVm();

        private void OnCryptoSelected(object selectedCrypto)
        {
            if (selectedCrypto is not Cryptocurrency crypto) return;

            SelectedCryptocurrency = crypto;
            _selectedItem.Id = _selectedCryptocurrency.Id;

            SearchQuery = _selectedCryptocurrency.Name;
            IsPopupOpen = false;

            if (CurrentView is DetailsVm detailsVm)
            {
                _ = detailsVm.UpdateDetails();
            }
            else
            {
                CurrentView = new DetailsVm();
            }
        }
        private void SearchCryptocurrencies(string id)
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                IsPopupOpen = false;
                return;
            }

            CryptocurrenciesSearched = SearchInList(id, _cryptocurrencies);
            IsPopupOpen = CryptocurrenciesSearched.Count > 0;
        }

        private static ObservableCollection<Cryptocurrency> SearchInList(string query, ObservableCollection<Cryptocurrency> cryptocurrencies)
        {
            var searchResult = new ObservableCollection<Cryptocurrency>();
            query = query.ToLower();

            var filteredCryptocurrencies = cryptocurrencies
                .Where(c => c.Name.ToLower().Contains(query))
                .OrderBy(c => !c.Name.ToLower().StartsWith(query))  
                .ThenBy(c => c.Name);  

            foreach (var cryptocurrency in filteredCryptocurrencies)
            {
                searchResult.Add(cryptocurrency);
            }

            return searchResult;
        }
    }
}