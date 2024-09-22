namespace Cryptocurrencies.Models
{
    internal class CoinCapResponse<T>
    {
        public List<T> Data { get; set; } = [];
    }
}