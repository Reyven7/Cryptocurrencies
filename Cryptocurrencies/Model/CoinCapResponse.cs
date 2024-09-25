namespace Cryptocurrencies.Model
{
    internal class CoinCapResponse<T>
    {
        public List<T> Data { get; set; } = [];
    }
}