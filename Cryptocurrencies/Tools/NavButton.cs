using System.Windows;
using System.Windows.Controls;

namespace Cryptocurrencies.Tools
{
    public class NavButton : RadioButton
    {
        static NavButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavButton), new FrameworkPropertyMetadata(typeof(NavButton)));
        }
    }
}