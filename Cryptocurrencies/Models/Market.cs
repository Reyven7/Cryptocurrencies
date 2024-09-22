namespace Cryptocurrencies.Models
{
    internal class Market
    {
        public string ExchangeId { get; set; } = string.Empty;
        public decimal? PriceUsd { get; set; }
    }
}