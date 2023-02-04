using System;
using System.Globalization;
using com.mahonkin.tim.maui.TeaTimer.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Media;

namespace com.mahonkin.tim.maui.TeaTimer.Converters
{
    /// <inheritdoc cref="IValueConverter"/>
    internal class TemperatureConverter : IValueConverter
    {
        #region Constants
        const string csym = "\u2103";
        const string fsym = "\u2109";
        #endregion Constants

        #region Private Fields
        private TeaSettingsService _settingService;
        #endregion Private Fields

        #region Constructors
        /// <Summary>Constructor</Summary>
        public TemperatureConverter()
        {
            _settingService = new TeaSettingsService();
        }
        #endregion Constructors

        #region Interface Methods
        /// <inheritdoc cref="IValueConverter.Convert(object, Type, object, CultureInfo)" />
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((_settingService.Get<bool>("UseCelsius", false)))
            {
                double celsius = UnitConverters.FahrenheitToCelsius((int)value);
                return celsius.ToString() + csym;
            }
            else
            {
                return value.ToString() + fsym;
            }
        }

        /// <inheritdoc cref="IValueConverter.ConvertBack(object, Type, object, CultureInfo)" />
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((_settingService.Get<bool>("UseCelsius", false)))
            {
                return (int)UnitConverters.CelsiusToFahrenheit(double.Parse((string)value));
            }
            else
            {
                return int.Parse((string)value);
            }
        }
        #endregion Interface Methods
    }
}