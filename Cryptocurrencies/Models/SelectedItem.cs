namespace Cryptocurrencies.Models
{
    public class SelectedItem
    {
        private static SelectedItem _instance;

        private SelectedItem() { }

        public static SelectedItem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SelectedItem();
                }
                return _instance;
            }
        }

        public string Item { get; set; } = string.Empty;
    }

}
