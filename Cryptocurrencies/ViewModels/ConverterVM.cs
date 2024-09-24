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
        private readonly HttpRequest _httpRequest = new();
        public ConverterVm()
        {
            Uri url = new($"https://api.coincap.io/v2/assets/");
             _ = _httpRequest.GetCryptocurrenciesAsync(url, CryptocurrenciesSearched);
            ConvertCommand = new RelayCommand(ConvertCommandExecuted);
        }

        private ObservableCollection<Cryptocurrency> _cryptocurrenciesSearched = [];
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

        public ICommand ConvertCommand { get; set; }

        private async void ConvertCommandExecuted(object obj)
        {
            if (Count > 0)
            {
                Uri request = new($"https://api.coincap.io/v2/assets/{CurrencyToConvert.Id}");
                Uri request2 = new($"https://api.coincap.io/v2/assets/{CurrencyConverted.Id}");

                var result = await _httpRequest.ConvertValue(request, request2, Count);
                Result = result?.ToString("F2");

            }
        }
    }
}