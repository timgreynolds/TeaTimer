using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using com.mahonkin.tim.maui.TeaTimer.Services;
using com.mahonkin.tim.TeaDataService.DataModel;
using com.mahonkin.tim.TeaDataService.Services;
using Microsoft.Extensions.Logging;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    /// <summary>
    /// Base class to be used for viewmodels.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged" />
        public event PropertyChangedEventHandler PropertyChanged;
        /// <inheritdoc cref="TeaNavigationService" />
        public readonly INavigationService NavigationService;
        /// <inheritdoc cref="TeaDisplayService" />
        public readonly IDisplayService DisplayService;
        /// <inheritdoc cref="TeaSqlService{T}"/>
        public readonly IDataService<TeaModel> SqlService;
        /// <inheritdoc cref="ISettingsService"/>
        public readonly ISettingsService SettingsService;

        /// <summary>
        /// Base class to be used for viewmodels.
        /// </summary>
        /// <param name="navigationService"><see cref="TeaNavigationService"/></param>
        /// <param name="displayService"><see cref="TeaDisplayService"/></param>
        /// <param name="sqlService"><see cref="TeaSqlService{TeaModel}"/></param>
        public BaseViewModel(INavigationService navigationService, IDisplayService displayService, IDataService<TeaModel> sqlService, ISettingsService settingsService, ILoggerFactory loggerFactory)
        {
            NavigationService = navigationService;
            DisplayService = displayService;
            SettingsService = settingsService;
            SqlService = sqlService;
        }

        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged" />
        public virtual void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value) == false)
            {
                storage = value;
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}