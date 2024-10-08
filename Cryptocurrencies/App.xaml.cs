﻿using LiveChartsCore;
using System.Windows;
using LiveChartsCore.SkiaSharpView;

namespace Cryptocurrencies
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            LiveCharts.Configure(config =>
                    config
                        // you can override the theme 
                        .AddDarkTheme()  

                        // In case you need a non-Latin based font, you must register a typeface for SkiaSharp
                        //.HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('汉')) // <- Chinese 
                        //.HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('あ')) // <- Japanese 
                        //.HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('헬')) // <- Korean 

                        //.HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('أ'))  // <- Arabic 
                        //.UseRightToLeftSettings() // Enables right to left tooltips 

                        // finally register your own mappers
                        // you can learn more about mappers at:
                        // https://livecharts.dev/docs/wpf/2.0.0-rc2/Overview.Mappers

                        // here we use the index as X, and the population as Y 
                        .HasMap<City>((city, index) => new(index, city.Population))
                // .HasMap<Foo>( .... ) 
                // .HasMap<Bar>( .... ) 
            );
        }

        public record City(string Name, double Population);
    }

}
