using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.mahonkin.tim.maui.TeaTimer.Services;
using com.mahonkin.tim.TeaDataService.DataModel;
using com.mahonkin.tim.TeaDataService.Exceptions;
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
        private ILogger _logger;
        #endregion Private Fields

        #region Public Properties
        /// <inheritdoc cref="Button.Text"/> 
        public string ButtonText
        {
            get => _buttonText;
            set => SetProperty(ref _buttonText, value);
        }

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

        /// <inheritdoc cref="Command"/>
        public Command TimerButtonPressed
        {
            get;
            private set;
        }
        #endregion Public Properties

        #region Constructor
        /// <inheritdoc cref="BaseViewModel"/>
        public TimerViewModel(INavigationService navigationService, IDisplayService displayService, IDataService<TeaModel> sqlService, ISettingsService settingsService, ILoggerFactory loggerFactory, ITimerService timerService)
            : base(navigationService, displayService, sqlService, settingsService)
        {
            _logger = loggerFactory.CreateLogger(typeof(TimerViewModel).FullName);
            _logger.LogTrace("Constructor entered.");
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
                _logger.LogCritical(ex.Message);
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
            else if (SelectedTea is null)
            {
                _timerService.Stop();
                CountdownLabel = TimeSpan.FromSeconds(0.0);
                IsViewLabelVisible = false;
                IsButtonEnabled = false;
                ButtonText = "Start";
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
            if (_timerService.IsRunning)
            {
                _timerService.Stop();
                ToggleTeaListNavigation(true);
                ButtonText = "Start";
            }
            else
            {
                ToggleTeaListNavigation(false);

                ButtonText = "Stop";

                try
                {
                    _timerService.Start(_countdownLabel);
                }
                catch (ApplicationException appEx)
                {
                    _logger.LogCritical("An unknown application error occurred. {Message}", appEx.Message);
                    DisplayService.ShowAlertAsync(appEx.Message, "An unknown application error occurred.");
                }
                catch (UnauthorizedAccessException)
                {
                    _logger.LogCritical("The user has not authorized notifications for this app.");
                    DisplayService.ShowAlertAsync("Not Authorized!", "The user has not authorized notifications for this app.");
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex.Message);
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
                ToggleTeaListNavigation(true);
            }
        }

        private static void ToggleTeaListNavigation(bool enabled)
        {
            IEnumerator<ShellSection> enumerator = AppShell.Current.Items[0].Items.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Route.Equals(nameof(Pages.TeaListPage)))
                {
                    enumerator.Current.IsEnabled = enabled;
                }
            }
        }

        private async Task ShellNavigated(object sender, EventArgs args)
        {
            ShellNavigatedEventArgs navArgs = args as ShellNavigatedEventArgs;
            _logger.LogDebug($"Shell navigation completed. {navArgs.Previous.Location} to {navArgs.Current.Location} for {navArgs.Source}");
            Page currentPage = ((AppShell)sender).CurrentPage ?? (AppShell)sender;
            if (currentPage.GetType().IsAssignableTo(typeof(Pages.TimerPage)))
            {
                currentPage.IsBusy = true;

                try
                {
                    await RefreshTeas();
                }
                catch (TeaSqlException ex)
                {
                    _logger.LogCritical("A dataabse error occurred. {Result} - {Message}", ex.Result, ex.Message);
                    await DisplayService.ShowAlertAsync(ex.GetType().Name, $"A database error occurred.\n{ex.Result} - \"{ex.Message}\" ");
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex.Message);
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
