using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using com.mahonkin.tim.maui.TeaTimer.DataModel;
using com.mahonkin.tim.maui.TeaTimer.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    public class TimerViewModel : BaseViewModel
    {
        #region Private Fields
        private string _buttonText = "Start";
        private string _viewTitle = string.Empty;
        private bool _isButtonEnabled;
        private bool _isViewLabelVisible;
        private TimeSpan _countdownLabel = new TimeSpan(0);
        private List<TeaModel> _teas = new List<TeaModel>();
        private IDispatcherTimer _countdown;
        private TeaModel _selectedTea;
        #endregion

        #region Public Properties
        public string ButtonText
        {
            get => _buttonText;
            set => SetProperty(ref _buttonText, value);
        }

        public string ViewTitle
        {
            get => _viewTitle;
            set => SetProperty(ref _viewTitle, value);
        }

        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set => SetProperty(ref _isButtonEnabled, value);
        }

        public bool IsViewLabelVisible
        {
            get => _isViewLabelVisible;
            set => SetProperty(ref _isViewLabelVisible, value);
        }

        public TimeSpan CountdownLabel
        {
            get => _countdownLabel;
            set => SetProperty(ref _countdownLabel, value);
        }

        public List<TeaModel> Teas
        {
            get => _teas;
            set => SetProperty(ref _teas, value);
        }

        public TeaModel SelectedTea
        {
            get => _selectedTea;
            set
            {
                SetProperty(ref _selectedTea, value);
                OnSelectedTeaChanged();
            }
        }

        public ICommand TimerButtonPressed
        {
            get;
            private set;
        }
        #endregion

        #region Constructor
        public TimerViewModel(TeaNavigationService navigationService, TeaDisplayService displayService, TeaDispatcherService dispatcherService, TeaSqlService sqlService)
            : base(navigationService, displayService, sqlService)
        {
            //_teas = sqlService.GetAsync().Result;
            _countdown = dispatcherService.CreateDispatcher().CreateTimer();
            _countdown.Interval = TimeSpan.FromSeconds(1);
            _countdown.IsRepeating = true;
            _countdown.Tick += (sender, e) => ExecuteTimer();
            TimerButtonPressed = new Command(() => ExecuteTimerButton(), () => TimerCanExecute());
            RefreshTeas();
        }
        #endregion

        #region Private Methods
        private async Task RefreshTeas()
        {
            Teas = await SqlService.GetAsync();
        }

        private void OnSelectedTeaChanged()
        {
            if (SelectedTea != null && _countdown.IsRunning == false)
            {
                CountdownLabel = SelectedTea.SteepTime;
                IsViewLabelVisible = true;
                IsButtonEnabled = true;
            }
            else if (SelectedTea != null && _countdown.IsRunning)
            {
                _countdown.Stop();
                CountdownLabel = SelectedTea.SteepTime;
                IsViewLabelVisible = true;
                IsButtonEnabled = true;
            }
            else
            {
                _countdown.Stop();
                IsViewLabelVisible = false;
                IsButtonEnabled = false;
            }
        }

        private void ExecuteTimerButton()
        {
            if (_countdown.IsRunning)
            {
                _countdown.Stop();
                ButtonText = "Start";
            }
            else
            {
                ButtonText = "Stop";
                _countdown.Start();
            }
        }

        private bool TimerCanExecute()
        {
            return _isButtonEnabled;
        }

        private void ExecuteTimer()
        {
            if (CountdownLabel.TotalSeconds > 0)
            {
                CountdownLabel = CountdownLabel.Subtract(TimeSpan.FromSeconds(1));
            }
            else
            {
                _countdown.Stop();
                IsButtonEnabled = false;
                ButtonText = "Start";
                CountdownLabel = SelectedTea.SteepTime;
            }
        }
        #endregion
    }
}