using System.Windows;
using System.Windows.Controls;

namespace Cryptocurrencies.Utilities
{
    public class NavButton : RadioButton
    {
        static NavButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavButton), new FrameworkPropertyMetadata(typeof(NavButton)));
        }
    }
}