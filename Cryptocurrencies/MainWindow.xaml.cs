using System.Windows;
using System.Windows.Input;

namespace Cryptocurrencies
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CollapseButton_Click(object sender, RoutedEventArgs e)
        {
            Window.WindowState = WindowState.Minimized;
        }

        private void FullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            Window.WindowState = Window.WindowState != WindowState.Maximized ? WindowState.Maximized : WindowState.Normal;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LightTheme_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {

                var themeDictionary = new ResourceDictionary();
                var handyControlSkin = new ResourceDictionary();
                var styleDictionary = new ResourceDictionary
                {
                    Source = new Uri("Styles/GlobalStyles.xaml", UriKind.Relative)
                };

                themeDictionary.Source = new Uri("Themes/Light.xaml", UriKind.Relative);
                handyControlSkin.Source = new Uri("pack://application:,,,/HandyControl;component/Themes/skindefault.xaml", UriKind.Absolute);

                App.Current.Resources.Clear();
                App.Current.Resources.MergedDictionaries.Add(styleDictionary);
                App.Current.Resources.MergedDictionaries.Add(handyControlSkin);
                App.Current.Resources.MergedDictionaries.Add(themeDictionary);


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error changing theme: {ex.Message}");
            }
        }

        private void DarkTheme_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {

                var themeDictionary = new ResourceDictionary();
                var handyControlSkin = new ResourceDictionary();
                var styleDictionary = new ResourceDictionary
                {
                    Source = new Uri("Styles/GlobalStyles.xaml", UriKind.Relative)
                };

                themeDictionary.Source = new Uri("Themes/Dark.xaml", UriKind.Relative);
                handyControlSkin.Source = new Uri("pack://application:,,,/HandyControl;component/Themes/skindark.xaml", UriKind.Absolute);

                App.Current.Resources.Clear();
                App.Current.Resources.MergedDictionaries.Add(styleDictionary);
                App.Current.Resources.MergedDictionaries.Add(handyControlSkin);
                App.Current.Resources.MergedDictionaries.Add(themeDictionary);


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error changing theme: {ex.Message}");
            }
        }
    }
}