using Cryptocurrencies.Tools;
using System.Windows;

namespace Cryptocurrencies.ViewModel
{
    internal class SettingsVm : ViewModelBase
    {
        private int _selectedTheme { get; set; }
        public int SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                _selectedTheme = value;
                ChangeTheme();
            }
        }

        public SettingsVm()
        {
            
        }

        public void ChangeTheme()
        {
            try
            {
                var themeDictionary = new ResourceDictionary();
                var handyControlSkin = new ResourceDictionary();
                var styleDictionary = new ResourceDictionary
                {
                    Source = new Uri("Styles/GlobalStyles.xaml", UriKind.Relative)
                };

                switch (SelectedTheme)
                    {
                        case 0:
                            themeDictionary.Source = new Uri("Themes/Light.xaml", UriKind.Relative);
                            handyControlSkin.Source = new Uri("pack://application:,,,/HandyControl;component/Themes/skindefault.xaml", UriKind.Absolute);
                            break;

                        case 1:
                            themeDictionary.Source = new Uri("Themes/Dark.xaml", UriKind.Relative);
                            handyControlSkin.Source = new Uri("pack://application:,,,/HandyControl;component/Themes/skindark.xaml", UriKind.Absolute);
                            break;

                        default:
                            throw new ArgumentException($"Unknown theme");
                    }

                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(styleDictionary);
                Application.Current.Resources.MergedDictionaries.Add(themeDictionary);
                Application.Current.Resources.MergedDictionaries.Add(handyControlSkin);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error changing theme: {ex.Message}");
            }
        }

    }
}
