﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;
using com.mahonkin.tim.maui.TeaTimer.Services;
using System;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public TeaNavigationService NavigationService { get; private set; }
        public TeaDisplayService DisplayService { get; private set; }
        public TeaSqlService SqlService { get; private set; }

        public BaseViewModel(TeaNavigationService navigationService, TeaDisplayService displayService, TeaSqlService sqlService)
        {
            NavigationService = navigationService;
            DisplayService = displayService;
            SqlService = sqlService;
        }

        public virtual void ShellNavigated(object sender, ShellNavigatedEventArgs args)
        {
            Console.WriteLine($"ShellNavigated event occurred.");
        }

        protected virtual void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value) == false)
            {
                storage = value;
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}