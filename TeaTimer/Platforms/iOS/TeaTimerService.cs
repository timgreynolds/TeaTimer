using com.mahonkin.tim.maui.TeaTimer.Platforms.iOS;
using System;
using Foundation;
using Microsoft.Maui.Dispatching;
using UserNotifications;
using System.Collections;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    /// <summary>
    /// Implementation of <see cref="ITimerService"/> that takes advantage of the
    /// iOS Notification Center for timer expiry if the app is backgrounded.
    /// </summary>
    public class TeaTimerService : ITimerService
    {
        #region Private Fields
        private ApplicationException _applicationException = null;
        private UNAuthorizationStatus _authorizationStatus = UNAuthorizationStatus.NotDetermined;
        private IDispatcherTimer _countdown = null;
        private static readonly UNUserNotificationCenter _currentCtr = UNUserNotificationCenter.Current;
        #endregion Private Fields

        #region Public Properties
        /// <inheritdoc cref="ITimerService.Interval" />
        public TimeSpan Interval
        {
            get => _countdown.Interval;
            set => _countdown.Interval = value;
        }

        /// <inheritdoc cref="ITimerService.IsRepeating" />
        public bool IsRepeating
        {
            get => _countdown.IsRepeating;
            set => _countdown.IsRepeating = value;
        }

        /// <inheritdoc cref="ITimerService.IsRunning" />
        public bool IsRunning => _countdown.IsRunning;
        #endregion Public Properties

        #region Event Handlers
        /// <inheritdoc cref="ITimerService.Tick" />
        public event EventHandler Tick
        {
            add => _countdown.Tick += value;
            remove => _countdown.Tick -= value;
        }
        #endregion Event Handlers

        #region Public Methods
        /// <summary>
        /// Creates an instance of an IDispatcherTimer associated with the
        /// current shell's Dispatcher.
        /// </summary>
        /// <remarks>
        /// This works for iOS as long as the app remains in the foreground for
        /// the entire countdown. If the phone goes to sleep or the app is put
        /// into the background while the countdown is running it will stop. In
        /// that case a local notification request can also be created using
        /// <see cref="Start(TimeSpan)"/>.
        /// </remarks>
        public void CreateTimer() => _countdown ??= AppShell.Current.Dispatcher.CreateTimer();

        /// <summary>
        /// Starts the timer without creating an associated local notification
        /// request.
        /// </summary>
        public void Start() => _countdown.Start();

        /// <summary>
        /// Starts the timer and creates a local notification request set to
        /// alert after <paramref name="duration">duration</paramref>.
        /// </summary>
        /// <remarks>
        /// The notification alert will only occur when the app is in the
        /// background and requires that the user has allowed notifications
        /// in the Settings app. If the user has not allowed notifications, no
        /// alert will be given when the app is in the background or the phone is
        /// asleep at timer expiry.
        /// </remarks>
        public void Start(TimeSpan duration)
        {
            _currentCtr.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound, ProcessAuthRequest);
            _currentCtr.GetNotificationSettings((settings) => _authorizationStatus = settings.AuthorizationStatus);

            if (_applicationException is not null)
            {
                throw _applicationException;
            }

            try
            {
                if (_authorizationStatus != UNAuthorizationStatus.Denied)
                {
                    CreateNotification(duration);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
            finally
            {
                Start();
            }
        }

        /// <summary>
        /// Stops the countdown and cancels any pending notification requests.
        /// </summary>
        public void Stop()
        {
            _countdown.Stop();
            _currentCtr.RemovePendingNotificationRequests(new[] { Constants.TIMER_REQUEST });
        }
        #endregion Public Methods

        #region Private Methods
        private void CreateNotification(TimeSpan countdown)
        {
            try
            {
                UNMutableNotificationContent content = new UNMutableNotificationContent()
                {
                    Title = Constants.TITLE,
                    Sound = UNNotificationSound.DefaultCriticalSound
                };
                UNTimeIntervalNotificationTrigger trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(countdown.TotalSeconds, false);
                UNNotificationRequest request = UNNotificationRequest.FromIdentifier(Constants.TIMER_REQUEST, content, trigger);
                _currentCtr.AddNotificationRequest(request, ErrorHandler);
            }
            catch (Exception ex)
            {
                ApplicationException applicationException = new ApplicationException(ex.Message, ex);
                applicationException.HelpLink = ex.HelpLink;
                foreach (DictionaryEntry entry in ex.Data)
                {
                    applicationException.Data.Add(entry.Key, entry.Value);
                }
                if (_applicationException is null)
                {
                    _applicationException = applicationException;
                }
                else
                {
                    throw new AggregateException("Multiple errors occurred.", new[] { _applicationException, applicationException });
                }
            }
            finally
            {
                if (_applicationException is not null)
                {
                    throw _applicationException;
                }
            }
        }

        private void ErrorHandler(NSError error)
        {
            if (error is not null)
            {
                _applicationException = new ApplicationException(error.LocalizedFailureReason);
                _applicationException.Data.Add(nameof(error.LocalizedRecoveryOptions), error.LocalizedRecoveryOptions);
                _applicationException.Data.Add(nameof(error.LocalizedRecoverySuggestion), error.LocalizedRecoverySuggestion);
            }
        }

        private void ProcessAuthRequest(bool auth, NSError error)
        {
            if (error is not null)
            {
                ErrorHandler(error);
            }
            if (auth)
            {
                _authorizationStatus = UNAuthorizationStatus.Authorized;
            }
            else
            {
                _authorizationStatus = UNAuthorizationStatus.NotDetermined;
            }
        }
        #endregion Private Methods
    }
}