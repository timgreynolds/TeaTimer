using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using com.mahonkin.tim.maui.TeaTimer.Services;
using com.mahonkin.tim.TeaDataService.Exceptions;
using com.mahonkin.tim.TeaDataService.DataModel;
using com.mahonkin.tim.TeaDataService.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    /// <summary>
    /// View model that backs up the <see cref="Pages.TimerPage">
    /// Timer/Countdown</see> page.
    /// </summary>
    public partial class TimerViewModel : BaseViewModel
    {
        #region Private Fields
        private string _buttonText = "Start";
        private string _viewTitle = string.Empty;
        private bool _isButtonEnabled;
        private bool _isViewLabelVisible;
        private TimeSpan _countdownLabel = new TimeSpan(0);
        private List<TeaModel> _teas = new List<TeaModel>();
        private ITimerService _timerService;
        private TeaModel _selectedTea;
        #endregion Private Fields

        #region Public Properties
        /// <inheritdoc cref="Button.Text"/> 
        public string ButtonText
        {
            get => _buttonText;
            set => SetProperty(ref _buttonText, value);
        }

        //public string ViewTitle
        //{
        //    get => _viewTitle;
        //    set => SetProperty(ref _viewTitle, value);
        //}

        /// <summary>
        /// Flag used to control the state of the Stop/Start button.
        /// </summary>
        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set => SetProperty(ref _isButtonEnabled, value);
        }

        /// <summary>
        /// Flag used to control whether or not the Tea detail label should be shown.
        /// </summary>
        public bool IsViewLabelVisible
        {
            get => _isViewLabelVisible;
            set => SetProperty(ref _isViewLabelVisible, value);
        }

        /// <summary>
        /// Label that displays the current minutes and seconds left in the countdown.
        /// </summary>
        public TimeSpan CountdownLabel
        {
            get => _countdownLabel;
            set => SetProperty(ref _countdownLabel, value);
        }

        /// <summary>
        /// Lisdt of teas that exist in the data source.
        /// </summary>
        public List<TeaModel> Teas
        {
            get => _teas;
            set => SetProperty(ref _teas, value);
        }

        /// <summary>
        /// Tea that was selected from the list.
        /// </summary>
        public TeaModel SelectedTea
        {
            get => _selectedTea;
            set
            {
                SetProperty(ref _selectedTea, value);
                OnSelectedTeaChanged();
            }
        }

        /// <inheritdoc cref="ICommand"/>
        public ICommand TimerButtonPressed
        {
            get;
            private set;
        }
        #endregion Public Properties

        #region Constructor
        /// <inheritdoc cref="BaseViewModel"/>
        public TimerViewModel(INavigationService navigationService, IDisplayService displayService, IDataService<TeaModel> sqlService, ISettingsService settingsService, ITimerService timerService)
            : base(navigationService, displayService, sqlService, settingsService)
        {
            _timerService = timerService;
            _timerService.CreateTimer();
            _timerService.Interval = TimeSpan.FromSeconds(1);
            _timerService.IsRepeating = true;
            _timerService.Tick += (sender, e) => ExecuteTimer();
            TimerButtonPressed = new Command(ExecuteTimerButton, TimerCanExecute);
            NavigationService.ShellNavigated += async (sender, args) => await ShellNavigated(sender, args);
        }
        #endregion Constructor

        #region Private Methods
        private async Task RefreshTeas()
        {
            try
            {
                Teas = await SqlService.GetAsync();
            }
            catch (TeaSqlException ex)
            {
                await DisplayService.ShowExceptionAsync(ex);
            }
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
                catch (Exception ex)
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
            if (currentPage.GetType().IsAssignableTo(typeof(Pages.TimerPage)))
            {
                currentPage.IsBusy = true;
                try
                {
                    Teas = await SqlService.GetAsync();
                }
                catch (TeaSqlException ex)
                {
                    AppShell.Logger.LogError($"A database error occurred. {ex.GetType().Name} - {ex.Result}: \"{ex.Message}\"");
                    await DisplayService.ShowAlertAsync(ex.GetType().Name, $" A database error occurred.\n{ex.Result} - \"{ex.Message}\" ");
                }
                catch (Exception ex)
                {
                    await DisplayService.ShowExceptionAsync(ex);
                }
                finally
                {
                    currentPage.IsBusy = false;
                }
            }
        }
        #endregion Private Methods

        #region Partial Properties and Methods
        private partial Task TimerExpired();
        #endregion Partial Properties and Methods
    }
}
