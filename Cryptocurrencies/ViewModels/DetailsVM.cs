﻿using Cryptocurrencies.Models;
using Cryptocurrencies.Utilities;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using System.Windows;

namespace Cryptocurrencies.ViewModels
{
    internal class DetailsVm : ViewModelBase
    {
        private readonly HttpRequest _httpRequest = new();
        public ISeries[]? Series { get; set; }

        private ObservableCollection<Cryptocurrency> _selectedCryptocurrency = [];
        public ObservableCollection<Cryptocurrency> SelectedCryptocurrency
        {
            get => _selectedCryptocurrency;
            set
            {
                _selectedCryptocurrency = value;

                OnPropertyChanged();
            }
        }

        private ObservableCollection<Market> _markets = [];
        public ObservableCollection<Market> Markets
        {
            get => _markets;
            set { _markets = value; OnPropertyChanged(); }
        }

        public DetailsVm()
        {
            if ((SelectedItem.Instance.SearchItem) == string.Empty) return;
            _ = LoadCryptocurrencyDetails(SelectedItem.Instance.SearchItem);
        }

        public async Task UpdateDetails()
        {
            await LoadCryptocurrencyDetails(SelectedItem.Instance.SearchItem);
        }

        public async Task LoadCryptocurrencyDetails(string id)
        {
            Uri request = new($"https://api.coincap.io/v2/assets/{id}");
            Uri marketRequest = new($"https://api.coincap.io/v2/assets/{id}/markets");


            await _httpRequest.GetCryptocurrency(request, SelectedCryptocurrency);
            OnPropertyChanged(nameof(SelectedCryptocurrency));
            await _httpRequest.GetMarkets(marketRequest, Markets);
            await LoadCandlestickData(id);

            MessageBox.Show(SelectedCryptocurrency[0].Name);
        }

        private async Task LoadCandlestickData(string id)
        {
            try
            {
                Uri request = new($"https://api.coingecko.com/api/v3/coins/{id}/ohlc?vs_currency=usd&days=7");
                var data = await _httpRequest.GetCandlestickData(request);

                var any = data.Any();

                if (!any || data.Count < 2)
                {
                    Series = [];
                    OnPropertyChanged(nameof(Series));
                    return;
                }

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}