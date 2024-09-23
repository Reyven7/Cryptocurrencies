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

        public string SearchItem { get; set; } = string.Empty;
        public string FirstItem { get; set; } = string.Empty;
        public string SecondItem { get; set; } = string.Empty;
    }
}