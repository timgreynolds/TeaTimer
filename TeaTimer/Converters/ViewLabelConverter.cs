using System;
using System.Globalization;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;

namespace com.mahonkin.tim.maui.TeaTimer.Converters
{
    ///<inheritdoc cref="IMultiValueConverter"></inheritdoc>
    internal class ViewLabelConverter : IMultiValueConverter
    {
        #region Constants
        const string csym = "\u2103";
        const string fsym = "\u2109";
        #endregion Constants

        #region Interface Methods
        ///<inheritdoc cref="IMultiValueConverter.Convert(object[], Type, object, CultureInfo)" />
        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string viewLabel = string.Empty;
            viewLabel += $"Tea: {values.OfType<string>().FirstOrDefault()}, ";
            viewLabel += $"Brew Temp: {ConvertBrewTemp(values.OfType<int>().FirstOrDefault())}, ";
            viewLabel += $"Steep Time: {values.OfType<TimeSpan>().FirstOrDefault().ToString(@"m\:ss")}";
            return viewLabel.Trim();
        }

        ///<inheritdoc cref="IMultiValueConverter.ConvertBack(object, Type[], object, CultureInfo)" />
        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
        #endregion Interface Methods

        #region Private Methods
        private string ConvertBrewTemp(int value)
        {
            if (Preferences.Get("useCelsiusKey", false))
            {
                return UnitConverters.FahrenheitToCelsius(value).ToString() + csym;
            }
            else
            {
                return value + fsym;
            }
        }
        #endregion Private Methods
    }
}