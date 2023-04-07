﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using com.mahonkin.tim.maui.TeaTimer.DataModel;
using com.mahonkin.tim.maui.TeaTimer.Services;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    public class EditViewModel : BaseViewModel, IQueryAttributable
    {
        #region Private Fields
        private static string _name;
        private static string _backButtonLabel = "Back";
        private static int _brewTemp = 212;
        private static bool _isPageDirty;
        private static TimeSpan _steepTime = TimeSpan.FromMinutes(2);
        private static TeaModel _tea = new TeaModel(string.Empty, _steepTime, _brewTemp);
        #endregion Private Fields

        #region Public Properties
        public string Name
        {
            get => _tea.Name;
            set
            {
                if (value != _tea.Name)
                {
                    IsPageDirty = true;
                    _tea.Name = value;
                }
                SetProperty(ref _name, value);
            }
        }

        public string BackButtonLabel
        {
            get => _backButtonLabel;
            private set { }
        }

        public int BrewTemp
        {
            get => _tea.BrewTemp;
            set
            {
                if (value != _tea.BrewTemp)
                {
                    IsPageDirty = true;
                    _tea.BrewTemp = value;
                }
                SetProperty(ref _brewTemp, value);
            }
        }

        public bool UseCelsius { get; }

        public bool IsPageDirty
        {
            get => _isPageDirty;
            private set => SetProperty(ref _isPageDirty, value);
        }

        public TimeSpan SteepTime
        {
            get => _tea.SteepTime;
            set
            {
                if (value != _tea.SteepTime)
                {
                    IsPageDirty = true;
                    _tea.SteepTime = value;
                }
                SetProperty(ref _steepTime, value);
            }
        }

        public ICommand BackButtonCommand
        {
            get;
            private set;
        }

        public ICommand SaveBtnPressed
        {
            get;
            private set;
        }
        #endregion Public Properties

        public EditViewModel(TeaNavigationService navigationService, TeaDisplayService displayService, TeaSqlService sqlService)
           : base(navigationService, displayService, sqlService)
        {
            SaveBtnPressed = new Command(async () => await Save());
            BackButtonCommand = new Command(() => NavigateBack());
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Tea", out object param))
            {
                if (param.GetType().IsAssignableTo(typeof(TeaModel)))
                {
                    _tea = param as TeaModel;
                }
                else
                {
                    _tea = new TeaModel(string.Empty);
                }
            }
            else
            {
                _tea = new TeaModel(string.Empty);
            }
            Name = _tea.Name;
            BrewTemp = _tea.BrewTemp;
            SteepTime = _tea.SteepTime;
            IsPageDirty = false;
        }

        private async Task Save()
        {
            if (_tea.Id > 0)
            {
                try
                {
                    _tea = await SqlService.UpdateAsync(_tea);
                    await DisplayService.ShowAlertAsync("Tea Updated!", $"{_tea.Name} (Id: {_tea.Id}) successfully updated.", "ÖK");
                }
                catch (Exception ex)
                {
                    await DisplayService.ShowExceptionAsync(ex);
                }
            }
            else
            {
                try
                {
                    _tea = await SqlService.AddAsync(_tea);
                    await DisplayService.ShowAlertAsync("Tea Added!", $"{_tea.Name} ({_tea.Id}) successfully updated.", "ÖK");
                }
                catch (Exception ex)
                {
                    await DisplayService.ShowExceptionAsync(ex);
                }
            }
        }

        private void NavigateBack()
        {
            NavigationService.GoBackAsync();
        }
    }
}