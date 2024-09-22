using Cryptocurrencies.Models;
using Cryptocurrencies.Utilities;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;

namespace Cryptocurrencies.ViewModels
{
    class HomeVm : ViewModelBase
    {
        public HomeVm() => Task.Run(LoadCryptocurrencyData);

        private ObservableCollection<Cryptocurrency> _cryptocurrencies;
        public ObservableCollection<Cryptocurrency> Cryptocurrencies
        {
            get => _cryptocurrencies;
            set
            {
                _cryptocurrencies = value;
                OnPropertyChanged();
            }
        }

        private static async Task<List<Cryptocurrency>> GetCryptocurrencyData()
        {
            try
            {
                using HttpClient client = new();
                var response = await client.GetStringAsync("https://api.coincap.io/v2/assets?limit=15");
                var coinCapResponse = JsonConvert.DeserializeObject<CoinCapResponse<Cryptocurrency>>(response);
                return coinCapResponse?.Data ?? [];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching cryptocurrency data: {ex.Message}");
                return [];
            }
        }

        public async Task LoadCryptocurrencyData()
        {
            var cryptocurrencies = await GetCryptocurrencyData();
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