using System;
using com.mahonkin.tim.logging;
using com.mahonkin.tim.maui.TeaTimer.Platforms.iOS;
using com.mahonkin.tim.maui.TeaTimer.ViewModels;
using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using UIKit;
using UserNotifications;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <summary>
/// A set of methods to manage shared behaviors for your app.   
/// </summary>
[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    #region Private Fields
    private TimeSpan _timeLeft = TimeSpan.Zero;
    private DateTime _backgroundedTime = DateTime.MinValue;
    private object _currentBindingContext;
    private nint _logPtr = OSLogger.Create(typeof(TeaTimerApp).FullName, typeof(AppDelegate).FullName);
    #endregion Private Fields

    /// <inheritdoc cref="MauiProgram.CreateMauiApp()"/>
    protected override MauiApp CreateMauiApp()
    {
        OSLogger.LogTrace(_logPtr, "Creating MauiApp instance.");
        return MauiProgram.CreateMauiApp().Result;
    }

    /// <summary>
    /// Method that is run when the app enters the background. 
    /// </summary>
    /// <remarks>
    /// Saves some app state.<br/>If the currently displayed page is the Timer Page
    /// and the countdown is running the time left in the countdown as well as
    /// the time the app went into the background are stored. This information
    /// can be used later by <see cref="WillEnterForeground(UIApplication)"/> to
    /// restore/reset appropriate app state.
    /// </remarks>
    public override void DidEnterBackground(UIApplication application)
    {
        OSLogger.LogTrace(_logPtr, "Application did enter background.");
        base.DidEnterBackground(application);

        _backgroundedTime = DateTime.UtcNow;
        _currentBindingContext = AppShell.Current.CurrentPage.BindingContext;

        if (_currentBindingContext.GetType().IsAssignableTo(typeof(TimerViewModel)))
        {
            if (((TimerViewModel)_currentBindingContext).IsTimerRunning)
            {
                _timeLeft = ((TimerViewModel)_currentBindingContext).CountdownLabel;
                OSLogger.LogInformation(_logPtr, $"Active timer countdown entering background at {_backgroundedTime.ToString("MM/dd/yy HH:mm:ss.fff")} with {_timeLeft.Minutes.ToString("D2")}:{_timeLeft.Seconds.ToString("D2")} remaining.");
            }
        }
    }

    /// <summary>
    /// Method that is run when the app is about to enter the foreground.
    /// </summary>
    /// <remarks>
    /// Restores previously saved app state.<br/>If the countdown was running
    /// when the app entered the background and has not expired while the app
    /// was in the background uses the saved data to calculate the current time
    /// remaining in the countdown and updates the timer appropriately.
    /// </remarks>
    public override void WillEnterForeground(UIApplication application)
    {
        OSLogger.LogTrace(_logPtr, "Application will enter foreground.");
        base.WillEnterForeground(application);

        DateTime awakeTime = DateTime.UtcNow;
        TimeSpan elapsedTime = awakeTime.Subtract(_backgroundedTime);

        if (_currentBindingContext.GetType().IsAssignableTo(typeof(TimerViewModel)))
        {
            if (((TimerViewModel)_currentBindingContext).IsTimerRunning)
            {
                OSLogger.LogInformation(_logPtr, $"Active timer countdown entering foreground at {awakeTime.ToString("MM/dd/yy HH:mm:ss.fff")} after {elapsedTime.Hours}:{elapsedTime.Minutes.ToString("D2")}:{elapsedTime.Seconds.ToString("D2")}.");
                if (elapsedTime < _timeLeft)
                {
                    OSLogger.LogInformation(_logPtr, $"Timer is still running.");
                    ((TimerViewModel)_currentBindingContext).CountdownLabel = _timeLeft.Subtract(elapsedTime);
                }
                else
                {
                    OSLogger.LogInformation(_logPtr, $"Timer expired; resetting user interface.");
                    ((TimerViewModel)_currentBindingContext).SelectedTea = null;
                    ((TimerViewModel)_currentBindingContext).IsButtonEnabled = false;
                    ((TimerViewModel)_currentBindingContext).ToggleTeaListNavigation(true);
                }
            }
        }
    }

    /// <inheritdoc cref="MauiUIApplicationDelegate.WillFinishLaunching(UIApplication, NSDictionary)"/>
    public override bool WillFinishLaunching(UIApplication application, NSDictionary launchOptions)
    {
        OSLogger.LogTrace(_logPtr, "Application will finish launching.");
        UNUserNotificationCenter.Current.Delegate = new NotificationCenterDelegate();
        return base.WillFinishLaunching(application, launchOptions);
    }

    /// <inheritdoc cref="MauiUIApplicationDelegate.FinishedLaunching(UIApplication, NSDictionary)"/>
    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        OSLogger.LogTrace(_logPtr, "Application did finish launching.");
        return base.FinishedLaunching(application, launchOptions);
    }
}
