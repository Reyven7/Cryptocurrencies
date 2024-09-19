using Cryptocurrencies.Utilities;
using System.Windows.Input;

namespace Cryptocurrencies.ViewModels
{
    class NavigationVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand HomeCommand {  get; set; }
        public ICommand DetailsCommand { get; set; }
        public ICommand ConventerCommand { get; set; }
        public ICommand SettingsCommand { get; set; }

        private void Home(object obg) => CurrentView = new HomeVM();
        private void Details(object obg) => CurrentView = new DetailsVM();
        private void Conventer(object obg) => CurrentView = new ConverterVM();
        private void Settings(object obg) => CurrentView = new SettingsVM();

        public NavigationVM()
        {
            HomeCommand = new RelayComand(Home);
            DetailsCommand = new RelayComand(Details);
            ConventerCommand = new RelayComand(Conventer);
            SettingsCommand = new RelayComand(Settings);

            CurrentView = new HomeVM();
        }
    }
}