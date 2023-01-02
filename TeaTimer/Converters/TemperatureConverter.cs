using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Media;

namespace com.mahonkin.tim.maui.TeaTimer.Converters
{
    /// <inheritdoc cref="IValueConverter"/>
    internal class TemperatureConverter : IValueConverter
    {
        /// <inheritdoc cref="IValueConverter.Convert(object, Type, object, CultureInfo)" />
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(App.UseCelsius)
            {
                return UnitConverters.FahrenheitToCelsius((int)value);
            }
            else
            {
                return value;
            }
        }

        /// <inheritdoc cref="IValueConverter.ConvertBack(object, Type, object, CultureInfo)" />
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(App.UseCelsius)
            {
                return UnitConverters.CelsiusToFahrenheit((int)value);
            }
            else
            {
                return value;
            }
        }
    }
}

