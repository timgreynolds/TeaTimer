using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer.Converters
{
    /// <inheritdoc cref="IValueConverter"/>
	internal class TimespanConverter : IValueConverter
	{
        #region Constructors
        ///<Summary>Constructor</Summary>
        public TimespanConverter()
        {
        }
        #endregion Constructors

        #region Interface Methods
        /// <inheritdoc cref="IValueConverter.Convert(object, Type, object, CultureInfo)" />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        /// <inheritdoc cref="IValueConverter.ConvertBack(object, Type, object, CultureInfo)" />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan steepTime = new TimeSpan();

            TimeSpan.TryParseExact((string)value, @"m\:ss", null, out steepTime);
                        
            return steepTime;
        }
        #endregion Interface Methods
    }
}

