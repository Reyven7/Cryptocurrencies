namespace Cryptocurrencies.Models
{
    internal class FinancialData(DateTime date, double high, double open, double close, double low)
    {
        public DateTime Date { get; set; } = date;
        public double High { get; set; } = high;
        public double Open { get; set; } = open;
        public double Close { get; set; } = close;
        public double Low { get; set; } = low;
    }
}
