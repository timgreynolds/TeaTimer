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
            if (App.UseCelsius)
            {
                double celsius = UnitConverters.FahrenheitToCelsius((int)value);
                return celsius.ToString();
            }
            else
            {
                return value.ToString();
            }
        }

        /// <inheritdoc cref="IValueConverter.ConvertBack(object, Type, object, CultureInfo)" />
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (App.UseCelsius)
            {
                return (int)UnitConverters.CelsiusToFahrenheit(double.Parse((string)value));
            }
            else
            {
                return int.Parse((string)value);
            }
        }
    }
}

