using Cryptocurrencies.Models;
using Cryptocurrencies.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;
using HandyControl.Controls;
using HandyControl.Tools.Extension;

namespace Cryptocurrencies.ViewModels
{
    internal class NavigationVm : ViewModelBase
    {
        private readonly HttpRequest _httpRequest = new();
        private readonly SelectedItem _selectedItem = SelectedItem.Instance;

        private ObservableCollection<Cryptocurrency> _cryptocurrenciesSearched;
        private object _currentView;
        private string _searchQuery;
        private bool _isPopupOpen;
        private Cryptocurrency _selectedCryptocurrency;

        public NavigationVm()
        {
            HomeCommand = new RelayCommand(Home);
            ConverterCommand = new RelayCommand(Converter);
            SettingsCommand = new RelayCommand(Settings);
            SelectCryptoCommand = new RelayCommand(OnCryptoSelected);

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
                SearchCryptocurrencies(SearchQuery);
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
        public ICommand SettingsCommand { get; set; }
        public ICommand SelectCryptoCommand { get; }

        private void Home(object obg) => CurrentView = new HomeVm();

        private void Converter(object obg) => CurrentView = new ConverterVm();

        private void Settings(object obg) => CurrentView = new SettingsVm();

        private void OnCryptoSelected(object selectedCrypto)
        {
            if (selectedCrypto is Cryptocurrency crypto)
            {
                SelectedCryptocurrency = crypto;
                _selectedItem.SearchItem = _selectedCryptocurrency.Id;
                if (CurrentView is DetailsVm detailsVm)
                {
                    CurrentView = new DetailsVm();
                }
                else
                {
                    CurrentView = new DetailsVm();
                }
            }
        }

        private async void SearchCryptocurrencies(string id)
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                IsPopupOpen = false;
                return;
            }

            CryptocurrenciesSearched = [];
            Uri url = new($"https://api.coincap.io/v2/assets?search={id}");
            await _httpRequest.GetCryptocurrenciesAsync(url, CryptocurrenciesSearched);
            IsPopupOpen = CryptocurrenciesSearched.Count > 0;
        }
    }
}