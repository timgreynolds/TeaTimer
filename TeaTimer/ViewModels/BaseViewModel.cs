﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using com.mahonkin.tim.maui.TeaTimer.Services;

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
        public readonly TeaNavigationService NavigationService;
        /// <inheritdoc cref="TeaDisplayService" />
        public readonly TeaDisplayService DisplayService;
        /// <inheritdoc cref="TeaSqlService{T}"/>
        public readonly TeaSqlService SqlService;

        /// <summary>
        /// Base class to be used for viewmodels.
        /// </summary>
        /// <param name="navigationService"><see cref="TeaNavigationService"/></param>
        /// <param name="displayService"><see cref="TeaDisplayService"/></param>
        /// <param name="sqlService"><see cref="TeaSqlService"/></param>
        public BaseViewModel(TeaNavigationService navigationService, TeaDisplayService displayService, TeaSqlService sqlService)
        {
            NavigationService = navigationService;
            DisplayService = displayService;
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