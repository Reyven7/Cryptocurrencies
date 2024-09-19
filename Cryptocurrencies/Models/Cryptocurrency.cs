using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptocurrencies.Models
{
    public class Cryptocurrency
    {
        public string ID {  get; set; }
        public string Name { get; set; }
        public decimal PriceUsd { get; set; }
        public decimal PriceChange { get; set; }
        public decimal ChangePercent24Hr { get; set; }
    }

    public class CoinCapResponse
    {
        public List<Cryptocurrency> Data { get; set; }
    }
}
