/**
* iOS platform-specific TeaTimerService implementation.
**/
using com.mahonkin.tim.maui.TeaTimer.Platforms.iOS;
using System;
using Foundation;
using Microsoft.Maui.Dispatching;
using UserNotifications;
using System.Collections;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public class TeaTimerService : ITimerService
    {
        ApplicationException _applicationException = null;
        private UNAuthorizationStatus _authorizationStatus = UNAuthorizationStatus.NotDetermined;
        private IDispatcherTimer _countdown = null;
        private static readonly UNUserNotificationCenter _currentCtr = UNUserNotificationCenter.Current;

        public TimeSpan Interval
        {
            get => _countdown.Interval;
            set => _countdown.Interval = value;
        }

        public bool IsRepeating
        {
            get => _countdown.IsRepeating;
            set => _countdown.IsRepeating = value;
        }

        public bool IsRunning => _countdown.IsRunning;

        public event EventHandler Tick
        {
            add => _countdown.Tick += value;
            remove => _countdown.Tick -= value;
        }

        public void CreateTimer()
        {
            _countdown = AppShell.Current.Dispatcher.CreateTimer();
        }

        public void Start()
        {
            _countdown.Start();
        }

        public void Start(TimeSpan duration)
        {
            _currentCtr.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound, ProcessAuthRequest);
            _currentCtr.GetNotificationSettings((settings) => _authorizationStatus = settings.AuthorizationStatus);

            if(_applicationException is not null)
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

        public void Stop()
        {
            _countdown.Stop();
            _currentCtr.RemovePendingNotificationRequests(new[] { Constants.TIMER_REQUEST });
        }

        private void CreateNotification(TimeSpan countdown)
        {
            try
            {
                UNMutableNotificationContent content = new UNMutableNotificationContent()
                {
                    CategoryIdentifier = Constants.TIMER_EXPIRED_CATEGORY,
                    Title = Constants.TITLE,
                    Subtitle = Constants.SUBTITLE,
                    Body = Constants.REQUEST_BODY,
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
                throw applicationException;
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
    }
}