using Cryptocurrencies.Models;
using Cryptocurrencies.Utilities;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace Cryptocurrencies.ViewModels
{
    internal class NavigationVm : ViewModelBase
    {
        private readonly SelectedItem _selectedItem = SelectedItem.Instance;

        private object _currentView;
        private ObservableCollection<Cryptocurrency> _cryptocurrenciesSearched;
        private string _searchQuery;
        private bool _isPopupOpen;
        private string _cryptoId;
        private Cryptocurrency _selectedCryptocurrency;

        public NavigationVm()
        {
            HomeCommand = new RelayCommand(Home);
            DetailsCommand = new RelayCommand(Details);
            ConverterCommand = new RelayCommand(Converter);
            SettingsCommand = new RelayCommand(Settings);

            CurrentView = new HomeVm();
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
                SearchCryptocurrencies();
            }
        }

        public Cryptocurrency SelectedCryptocurrency
        {
            get => _selectedCryptocurrency;
            set
            {
                _selectedCryptocurrency = value;
                OnPropertyChanged();
                if (_selectedCryptocurrency != null)
                {
                    CryptoId = _selectedCryptocurrency.Id;
                    ClosePopupAndReset();
                    CurrentView = new DetailsVm();
                }
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

        public string CryptoId
        {
            get => _cryptoId;
            set
            {
                _cryptoId = value;
                OnPropertyChanged();
                if (DetailsCommand.CanExecute(null))
                {
                    DetailsCommand.Execute(null);
                }
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
        public ICommand DetailsCommand { get; set; }
        public ICommand ConverterCommand { get; set; }
        public ICommand SettingsCommand { get; set; }

        private void Home(object obg) => CurrentView = new HomeVm();

        private void Converter(object obg) => CurrentView = new ConverterVm();

        private void Settings(object obg) => CurrentView = new SettingsVm();

        private void Details(object obg)
        {
            if (!string.IsNullOrEmpty(CryptoId))
            {
                _selectedItem.SearchItem = CryptoId;
                CurrentView = new DetailsVm();
            }
            else
            {
                MessageBox.Show("Please select a valid cryptocurrency.");
            }
        }

        private async void SearchCryptocurrencies()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                IsPopupOpen = false;
                return;
            }

            var searchResults = await GetCryptocurrencyBySearch(SearchQuery);
            CryptocurrenciesSearched = new ObservableCollection<Cryptocurrency>(searchResults);
            IsPopupOpen = searchResults.Count > 0;
        }

        private static async Task<List<Cryptocurrency>> GetCryptocurrencyBySearch(string query)
        {
            try
            {
                using HttpClient client = new();
                var response = await client.GetStringAsync($"https://api.coincap.io/v2/assets?search={query}");
                var coinCapResponse = JsonConvert.DeserializeObject<CoinCapResponse<Cryptocurrency>>(response);
                return coinCapResponse?.Data ?? [];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching cryptocurrency data: {ex.Message}");
                return [];
            }
        }

        private void ClosePopupAndReset()
        {
            IsPopupOpen = false;
            SearchQuery = _selectedCryptocurrency.Name;
            OnPropertyChanged(nameof(SearchQuery));

            _selectedCryptocurrency = null;
            OnPropertyChanged(nameof(SelectedCryptocurrency));
        }
    }
}