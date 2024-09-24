using Cryptocurrencies.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace Cryptocurrencies.Utilities
{
    internal class HttpRequest
    {
        public async Task GetCryptocurrenciesAsync(Uri request, ObservableCollection<Cryptocurrency> cryptocurrencies)
        {
            try
            {
                using HttpClient client = new();
                var response = await client.GetStringAsync(request);
                var coinCapResponse = JsonConvert.DeserializeObject<CoinCapResponse<Cryptocurrency>>(response);
                if (coinCapResponse?.Data != null)
                {
                    cryptocurrencies.Clear();
                    foreach (var cryptocurrency in coinCapResponse.Data)
                    {
                        cryptocurrencies.Add(cryptocurrency);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cryptocurrency details: {ex.Message}");
            }
        }

        public async Task<List<Cryptocurrency>> GetCryptocurrenciesAsync(Uri request)
        {
            try
            {
                using HttpClient client = new();
                var response = await client.GetStringAsync(request);
                var coinCapResponse = JsonConvert.DeserializeObject<CoinCapResponse<Cryptocurrency>>(response);
                return coinCapResponse?.Data ?? [];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching cryptocurrency data: {ex.Message}");
                return [];
            }
        }

        public async Task GetCryptocurrency(Uri request, ObservableCollection<Cryptocurrency> cryptocurrencies)
        {
            try
            {
                using HttpClient client = new();
                var response = await client.GetStringAsync(request);
                var coinCapSingleResponse =
                    JsonConvert.DeserializeObject<CoinCapSingleResponse<Cryptocurrency>>(response);
                if (coinCapSingleResponse?.Data != null)
                {
                    cryptocurrencies.Clear();
                    cryptocurrencies.Add(coinCapSingleResponse.Data);
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cryptocurrency details: {ex.Message}");
            }
        }

        public async Task<Cryptocurrency> GetCryptocurrency(Uri request)
        {
            try
            {
                using HttpClient client = new();
                var response = await client.GetStringAsync(request);
                var coinCapSingleResponse =
                    JsonConvert.DeserializeObject<CoinCapSingleResponse<Cryptocurrency>>(response);
                return coinCapSingleResponse?.Data;
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cryptocurrency details: {ex.Message}");
            }

            return null;
        }

        public async Task GetMarkets(Uri request, ObservableCollection<Market> markets)
        {
            try
            {
                using HttpClient client = new();
                var response = await client.GetStringAsync(request);
                var coinCapResponse = JsonConvert.DeserializeObject<CoinCapResponse<Market>>(response);
                if (coinCapResponse?.Data != null)
                {
                    markets.Clear();
                    foreach (var market in coinCapResponse.Data)
                    {
                        markets.Add(market);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cryptocurrency markets: {ex.Message}");
            }
        }

        public async Task<List<Market>> GetMarkets(Uri request)
        {
            using HttpClient client = new();
            var response = await client.GetStringAsync(request);
            var coinCapResponse = JsonConvert.DeserializeObject<CoinCapResponse<Market>>(response);
            return coinCapResponse?.Data ?? [];

        }
        public async Task<List<FinancialData>> GetCandlestickData(Uri request)
        {
            var financialDataList = new List<FinancialData>();

            try
            {
                using HttpClient client = new();
                var response = await client.GetStringAsync(request);
                var rawData = JsonConvert.DeserializeObject<List<List<object>>>(response);

                if (rawData == null || !rawData.Any())
                {
                    return financialDataList;
                }

                foreach (var item in rawData)
                {
                    var date = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(item[0])).DateTime;
                    var open = Convert.ToDouble(item[1]);
                    var high = Convert.ToDouble(item[2]);
                    var low = Convert.ToDouble(item[3]);
                    var close = Convert.ToDouble(item[4]);

                    financialDataList.Add(new FinancialData(date, high, open, close, low));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return financialDataList;
        }

        public async Task<decimal?> ConvertValue(Uri request, Uri request2, int count)
        {
            try
            {
                var currencyToConvert = await GetCryptocurrency(request);
                var currencyToConvert1 = await GetCryptocurrency(request2);

                return (currencyToConvert.PriceUsd / currencyToConvert1.PriceUsd) * count;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching cryptocurrency data: {ex.Message}");
            }
            return null;
        }
    }
}