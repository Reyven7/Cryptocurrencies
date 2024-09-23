using Cryptocurrencies.Models;
using Cryptocurrencies.Utilities;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace Cryptocurrencies.ViewModels
{
    internal class ConverterVm : ViewModelBase
    {
        public ConverterVm()
        {
            SearchCryptocurrencies();
            ConvertCommand = new RelayCommand(ConvertCommandExecuted);
        }

        private ObservableCollection<Cryptocurrency> _cryptocurrenciesSearched;
        private Cryptocurrency _currencyToConvert;
        private Cryptocurrency _currencyConverted;
        private int _count;
        private string? _result;

        public string? Result
        {
            get => _result;
            set { _result = $"{Count} {_currencyToConvert.Name} = {value}, {_currencyConverted.Name}"; OnPropertyChanged();}
        }

        public int Count
        {
            get => _count;
            set { _count = value; OnPropertyChanged(); }
        }

        public Cryptocurrency CurrencyToConvert
        {
            get => _currencyToConvert;
            set
            {
                _currencyToConvert = value;
                OnPropertyChanged();
            }
        }

        public Cryptocurrency CurrencyConverted
        {
            get => _currencyConverted;
            set
            {
                _currencyConverted = value;
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

        private async void SearchCryptocurrencies()
        {
            var searchResults = await GetCryptocurrency();
            CryptocurrenciesSearched = new ObservableCollection<Cryptocurrency>(searchResults);
        }

        private static async Task<List<Cryptocurrency>> GetCryptocurrency()
        {
            try
            {
                using HttpClient client = new();
                var response = await client.GetStringAsync("https://api.coincap.io/v2/assets");
                var coinCapResponse = JsonConvert.DeserializeObject<CoinCapResponse<Cryptocurrency>>(response);
                return coinCapResponse?.Data ?? [];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching cryptocurrency data: {ex.Message}");
                return [];
            }
        }

        public ICommand ConvertCommand { get; set; }

        private void ConvertCommandExecuted(object obj)
        {
            if (Count > 0)
            {
                Convert(CurrencyToConvert.Id, CurrencyConverted.Id, Count);
            }
        }
        private async void Convert(string id, string currency, int count)
        {
            try
            {
                using HttpClient client = new();
                var response = await client.GetStringAsync($"https://api.coincap.io/v2/assets/{id}");
                var coinCapSingleResponse = JsonConvert.DeserializeObject<CoinCapSingleResponse<Cryptocurrency>>(response);
                var currencyToConvert = coinCapSingleResponse?.Data?.PriceUsd;

                MessageBox.Show(currencyToConvert.ToString());


                using HttpClient client1 = new();
                var response1 = await client1.GetStringAsync($"https://api.coincap.io/v2/assets/{currency}");
                var coinCapSingleResponse1 = JsonConvert.DeserializeObject<CoinCapSingleResponse<Cryptocurrency>>(response1);
                var currencyToConvert1 = coinCapSingleResponse1?.Data?.PriceUsd;



                Result = (currencyToConvert!.Value / currencyToConvert1!.Value * count).ToString("F2");
                MessageBox.Show(Result);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching cryptocurrency data: {ex.Message}");
            }
        }
    }
}