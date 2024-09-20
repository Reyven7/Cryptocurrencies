namespace Cryptocurrencies.Models
{
    public class Cryptocurrency
    {
        public string ID {  get; set; }
        public string Rank { get; set; }
        public string Name { get; set; }
        public decimal? PriceUsd { get; set; }
        public decimal? Supply { get; set; }
        public decimal? Vwap24Hr { get; set; }
        public float? ChangePercent24Hr { get; set; }
    }

    public class CoinCapResponse(List<Cryptocurrency> data)
    {
        public List<Cryptocurrency> Data { get; set; } = data;
    }
}
