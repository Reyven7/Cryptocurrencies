using Cryptocurrencies.Models;
using Cryptocurrencies.Utilities;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace Cryptocurrencies.ViewModels
{
    class NavigationVm : ViewModelBase
    {
        public NavigationVm()
        {
            HomeCommand = new RelayCommand(Home);
            DetailsCommand = new RelayCommand(Details);
            ConverterCommand = new RelayCommand(Converter);
            SettingsCommand = new RelayCommand(Settings);

            CurrentView = new HomeVm();
        }

        private object _currentView;
        private ObservableCollection<Cryptocurrency> _cryptocurrenciesSearched;
        private string _searchQuery;
        private bool _isPopupOpen;

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
                OnPropertyChanged(nameof(SearchQuery));
                SearchCryptocurrencies();
            }
        }

        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set
            {
                _isPopupOpen = value;
                OnPropertyChanged(nameof(IsPopupOpen));
            }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand DetailsCommand { get; set; }
        public ICommand ConverterCommand { get; set; }
        public ICommand SettingsCommand { get; set; }

        private void Home(object obg) => CurrentView = new HomeVm();
        private void Details(object obg) => CurrentView = new DetailsVm();
        private void Converter(object obg) => CurrentView = new ConverterVm();
        private void Settings(object obg) => CurrentView = new SettingsVm();

        private static async Task<List<Cryptocurrency>> GetCryptocurrencyBySearch(string query)
        {
            try
            {
                using HttpClient client = new();
                var response = await client.GetStringAsync($"https://api.coincap.io/v2/assets?search={query}");
                var coinCapResponse = JsonConvert.DeserializeObject<CoinCapResponse>(response);
                return coinCapResponse?.Data ?? [];

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching cryptocurrency data: {ex.Message}");
                return [];
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
    }
}