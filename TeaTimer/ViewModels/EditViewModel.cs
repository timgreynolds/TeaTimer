using System;
using System.Collections.Generic;
using System.Windows.Input;
using com.mahonkin.tim.maui.TeaTimer.DataModel;
using com.mahonkin.tim.maui.TeaTimer.Services;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    public class EditViewModel : BaseViewModel, IQueryAttributable
    {
        #region Private Fields
        private string _name;
        private int _brewTemp;
        private bool _isPageDirty;
        private TimeSpan _steepTime;
        #endregion Private Fields

        #region Public Properties
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public int BrewTemp
        {
            get => _brewTemp;
            set
            {
                if (value != _brewTemp)
                {
                    IsPageDirty = true;
                }
                SetProperty(ref _brewTemp, value);
            }
        }

        public bool IsPageDirty
        {
            get => _isPageDirty;
            set => SetProperty(ref _isPageDirty, value);
        }

        public TimeSpan SteepTime
        {
            get => _steepTime;
            set
            {
                if (value != _steepTime)
                {
                    IsPageDirty = true;
                }
                SetProperty(ref _steepTime, value);
            }
        }

        public ICommand SaveBtnPressed
        {
            get;
            private set;
        }
        #endregion Public Properties

        public EditViewModel(INavigationService navigationService, IDisplayService displayService) : base(navigationService, displayService)
        {
            SaveBtnPressed = new Command(() => Save());
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count <= 0)
            {
                query.Add("Tea", new TeaModel(string.Empty));
            }
            if (query.TryGetValue("Tea", out object param))
            {
                if (param.GetType().IsAssignableTo(typeof(TeaModel)))
                {
                    TeaModel tea = param as TeaModel;
                    Name = tea.Name;
                    BrewTemp = tea.BrewTemp;
                    SteepTime = tea.SteepTime;
                    IsPageDirty = false;
                }
                else
                {
                    DisplayService.ShowAlertAsync("Error", "Query parmater could not be interpreted as a Tea.");
                }
            }
            else
            {
                DisplayService.ShowAlertAsync("Error", "No query parameter matching 'Tea' was passed.");
            }
        }

        private void Save()
        {
            DisplayService.ShowAlertAsync("Action", "Save button pressed");
        }
    }
}