using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using com.mahonkin.tim.maui.TeaTimer.DataModel;
using com.mahonkin.tim.maui.TeaTimer.Services;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    public partial class TimerViewModel : BaseViewModel
    {
        #region Private Fields
        private string _buttonText = "Start";
        private string _viewTitle = string.Empty;
        private bool _isButtonEnabled;
        private bool _isViewLabelVisible;
        private TimeSpan _countdownLabel = new TimeSpan(0);
        private List<TeaModel> _teas = new List<TeaModel>();
        private TeaTimerService _timerService;
        private TeaModel _selectedTea;
        #endregion Private Fields

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
        #endregion Public Properties

        #region Constructor
        public TimerViewModel(TeaNavigationService navigationService, TeaDisplayService displayService, TeaSqlService sqlService, TeaTimerService timerService)
            : base(navigationService, displayService, sqlService)
        {
            _timerService = timerService;
            _timerService.CreateTimer();
            _timerService.Interval = TimeSpan.FromSeconds(1);
            _timerService.IsRepeating = true;
            _timerService.Tick += (sender, e) => ExecuteTimer();
            TimerButtonPressed = new Command(ExecuteTimerButton, TimerCanExecute);
            navigationService.ShellNavigated += async (sender, args) => await ShellNavigated(sender, args);
        }
        #endregion Constructor

        #region Private Methods
        private async Task RefreshTeas()
        {
            Teas = await SqlService.GetAsync();
        }

        private void OnSelectedTeaChanged()
        {
            if (SelectedTea != null && _timerService.IsRunning == false)
            {
                CountdownLabel = SelectedTea.SteepTime;
                IsViewLabelVisible = true;
                IsButtonEnabled = true;
            }
            else if (SelectedTea != null && _timerService.IsRunning)
            {
                _timerService.Stop();
                CountdownLabel = SelectedTea.SteepTime;
                IsViewLabelVisible = true;
                IsButtonEnabled = true;
            }
            else
            {
                _timerService.Stop();
                CountdownLabel = TimeSpan.FromSeconds(0.0);
                IsViewLabelVisible = false;
                IsButtonEnabled = true;
            }
        }

        private void ExecuteTimerButton()
        {
            IEnumerator<ShellSection> enumerator = AppShell.Current.Items[0].Items.GetEnumerator();
            if (_timerService.IsRunning)
            {
                _timerService.Stop();
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.IsEnabled == false)
                    {
                        enumerator.Current.IsEnabled = true;
                    }
                }
                ButtonText = "Start";
            }
            else
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.Route.Equals(nameof(Pages.TeaListPage)))
                    {
                        enumerator.Current.IsEnabled = false;
                    }
                }

                ButtonText = "Stop";

                try
                {
                    _timerService.Start(_countdownLabel);
                }
                catch (ApplicationException appEx)
                {
                    DisplayService.ShowAlertAsync(appEx.Message, "An unknown application error occurred.");
                }
                catch (UnauthorizedAccessException)
                {
                    DisplayService.ShowAlertAsync("Not Authorized!", "The user has not authorized notifications for this app.");
                }
                catch(Exception ex)
                {
                    DisplayService.ShowExceptionAsync(ex);
                }
            }
        }

        private bool TimerCanExecute()
        {
            return _isButtonEnabled;
        }

        private void ExecuteTimer()
        {
            if (CountdownLabel.TotalSeconds > 0.0)
            {
                CountdownLabel = CountdownLabel.Subtract(TimeSpan.FromSeconds(1));
            }
            else
            {
                _timerService.Stop();
                TimerExpired();
                SelectedTea = null;
                ButtonText = "Start";
                IsButtonEnabled = false;
                IEnumerator<ShellSection> enumerator = AppShell.Current.Items[0].Items.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.IsEnabled == false)
                    {
                        enumerator.Current.IsEnabled = true;
                    }
                }
            }
        }

        private async Task ShellNavigated(object sender, EventArgs args)
        {
            Page currentPage = ((AppShell)sender).CurrentPage ?? (AppShell)sender;
            currentPage.IsBusy = true;
            Teas = await SqlService.GetAsync();
            currentPage.IsBusy = false;
        }
        #endregion Private Methods

        #region Partial Properties and Methods
        private partial Task TimerExpired();
        #endregion Partial Properties and Methods
    }
}
