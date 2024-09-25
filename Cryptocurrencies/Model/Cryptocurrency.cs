namespace Cryptocurrencies.Model
{
    public class Cryptocurrency
    {
        public string Id { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal? PriceUsd { get; set; }
        public decimal? Supply { get; set; }
        public decimal? Vwap24Hr { get; set; }
        public float? ChangePercent24Hr { get; set; }
        public decimal? MaxSupply { get; set; }
        public decimal? VolumeUsd24Hr { get; set; }
    }
}