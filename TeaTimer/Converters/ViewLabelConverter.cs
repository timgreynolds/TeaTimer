using System;
using System.Globalization;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Media;

namespace com.mahonkin.tim.maui.TeaTimer.Converters
{
	internal class ViewLabelConverter : IMultiValueConverter
    {
        const string csym = "\u2103";
        const string fsym = "\u2109";

        public ViewLabelConverter() 
		{
		}

        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string viewLabel = string.Empty;
            viewLabel += $"Tea: {values.OfType<string>().FirstOrDefault()}, ";
            viewLabel += $"Brew Temp: {ConvertBrewTemp(values.OfType<int>().FirstOrDefault())}, ";
            viewLabel += $"Steep Time: {values.OfType<TimeSpan>().FirstOrDefault().ToString(@"mm\:ss")}";
            return viewLabel.Trim();
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

        private string ConvertBrewTemp(int value)
        {
            if(App.UseCelsius)
            {
                return UnitConverters.FahrenheitToCelsius(value).ToString() + csym;
            }
            else
            {
                return value + fsym;
            }
        }
    }
}

