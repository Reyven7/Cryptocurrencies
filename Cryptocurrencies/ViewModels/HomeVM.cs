using Cryptocurrencies.Models;
using Cryptocurrencies.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cryptocurrencies.ViewModels
{
    class HomeVM : ViewModelBase
    {
        public HomeVM() => LoadCryptocurrencyData();

        private ObservableCollection<Cryptocurrency> _cryptocurrencies;
        public ObservableCollection<Cryptocurrency> Cryptocurrencies
        {
            get => _cryptocurrencies;
            set
            {
                _cryptocurrencies = value;
                OnPropertyChanged(nameof(Cryptocurrencies));
            }
        }

        private static async Task<List<Cryptocurrency>> GetCryptocurrencyData()
        {
            using HttpClient client = new();
            var response = await client.GetStringAsync("https://api.coincap.io/v2/assets?limit=10");
            var coinCapResponse = JsonConvert.DeserializeObject<CoinCapResponse>(response);
            return coinCapResponse?.Data ?? [];
        }

        public async Task LoadCryptocurrencyData()
        {
            var cryptocurrencies = await GetCryptocurrencyData();
            if (cryptocurrencies != null && cryptocurrencies.Count > 0)
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
