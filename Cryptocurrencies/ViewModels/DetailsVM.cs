using Cryptocurrencies.Models;
using Cryptocurrencies.Utilities;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;

namespace Cryptocurrencies.ViewModels
{
    internal class DetailsVm : ViewModelBase
    {
        private readonly string _cryptoId = SelectedItem.Instance.SearchItem;

        public DetailsVm()
        {
            if (_cryptoId == string.Empty) return;
            GetCryptocurrencyById(_cryptoId);
            LoadMarketById();
        }

        public ISeries[]? Series { get; set; }

        private ObservableCollection<Cryptocurrency>? _selectedCryptocurrency;
        public ObservableCollection<Cryptocurrency>? SelectedCryptocurrency
        {
            get => _selectedCryptocurrency;
            set
            {
                _selectedCryptocurrency = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Market>? _markets;
        public ObservableCollection<Market>? Markets
        {
            get => _markets;
            set { _markets = value; OnPropertyChanged(); }
        }

        private async void GetCryptocurrencyById(string id)
        {
            try
            {
                using HttpClient client = new();
                var response = await client.GetStringAsync($"https://api.coincap.io/v2/assets/{id}");
                var coinCapSingleResponse = JsonConvert.DeserializeObject<CoinCapSingleResponse<Cryptocurrency>>(response);
                SelectedCryptocurrency = [coinCapSingleResponse?.Data];

                await LoadCandlestickData();
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cryptocurrency details: {ex.Message}");
            }
        }

        private static async Task<List<Market>> GetMarketById(string id)
        {
            using HttpClient client = new();
            var response = await client.GetStringAsync($"https://api.coincap.io/v2/assets/{id}/markets");
            var coinCapResponse = JsonConvert.DeserializeObject<CoinCapResponse<Market>>(response);
            return coinCapResponse?.Data ?? [];
        }

        private async void LoadMarketById()
        {
            var searchResults = await GetMarketById(_cryptoId);
            Markets = new ObservableCollection<Market>(searchResults);
        }

        private static async Task<List<FinancialData>> GetCandlestickData(string id)
        {
            using HttpClient client = new();
            var response = await client.GetStringAsync($"https://api.coingecko.com/api/v3/coins/{id}/ohlc?vs_currency=usd&days=7");
            var rawData = JsonConvert.DeserializeObject<List<List<object>>>(response);

            var financialDataList = new List<FinancialData>();
            if (rawData == null) return financialDataList;

            foreach (var item in rawData)
            {
                var date = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(item[0])).DateTime;
                var open = Convert.ToDouble(item[1]);
                var high = Convert.ToDouble(item[2]);
                var low = Convert.ToDouble(item[3]);
                var close = Convert.ToDouble(item[4]);

                financialDataList.Add(new FinancialData(date, high, open, close, low));
            }
            return financialDataList;
        }

        private async Task LoadCandlestickData()
        {
            var data = await GetCandlestickData(_cryptoId);

            Series =
            [
                new CandlesticksSeries<FinancialPointI>
                {
                    Values = data
                        .Select(x => new FinancialPointI(x.High, x.Open, x.Close, x.Low))
                        .ToArray(),
                }
            ];

            OnPropertyChanged(nameof(Series));
        }
    }
}