using Cryptocurrencies.Model;
using Cryptocurrencies.Tools;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Cryptocurrencies.ViewModel
{
    internal class ConverterVm : ViewModelBase
    {
        public ConverterVm()
        {
            Uri url = new($"https://api.coincap.io/v2/assets/");
            _ = HttpRequest.GetCryptocurrenciesAsync(url, CryptocurrenciesSearched);
            ConvertCommand = new RelayCommand(ConvertCommandExecuted);
        }

        private ObservableCollection<Cryptocurrency> _cryptocurrenciesSearched = [];
        private Cryptocurrency _currencyToConvert = null!;
        private Cryptocurrency _currencyConverted = null!;
        private int _count;
        private string? _result;

        public string? Result
        {
            get => _result;
            set { _result = $"{Count} {_currencyToConvert.Name} = {value}, {_currencyConverted.Name}"; OnPropertyChanged(); }
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
            if (Count <= 0) return;

            Uri request = new($"https://api.coincap.io/v2/assets/{CurrencyToConvert.Id}");
            Uri request2 = new($"https://api.coincap.io/v2/assets/{CurrencyConverted.Id}");

            var result = await HttpRequest.ConvertValue(request, request2, Count);
            Result = result?.ToString("F2");
        }
    }
}