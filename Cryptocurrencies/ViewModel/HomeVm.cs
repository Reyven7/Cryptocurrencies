using Cryptocurrencies.Model;
using Cryptocurrencies.Tools;
using System.Collections.ObjectModel;
using System.Windows;

namespace Cryptocurrencies.ViewModel
{
    internal class HomeVm : ViewModelBase
    {
        public HomeVm() => Task.Run(LoadCryptocurrencyData);

        private ObservableCollection<Cryptocurrency> _cryptocurrencies = null!;

        public ObservableCollection<Cryptocurrency> Cryptocurrencies
        {
            get => _cryptocurrencies;
            set
            {
                _cryptocurrencies = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadCryptocurrencyData()
        {
            Uri request = new("https://api.coincap.io/v2/assets?limit=15");

            var cryptocurrencies = await HttpRequest.GetCryptocurrenciesAsync(request);
            if (cryptocurrencies is { Count: > 0 })
            {
                Cryptocurrencies = new ObservableCollection<Cryptocurrency>(cryptocurrencies);
            }
            else
            {
                MessageBox.Show("No data received from the API.");
            }
        }
    }
}