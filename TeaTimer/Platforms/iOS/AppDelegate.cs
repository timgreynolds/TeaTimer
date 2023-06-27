using System;
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
    #endregion Private Fields

    /// <inheritdoc cref="MauiProgram.CreateMauiApp()"/>
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

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
        _backgroundedTime = DateTime.UtcNow;
        _currentBindingContext = AppShell.Current.CurrentPage.BindingContext;

        if (_currentBindingContext.GetType().IsAssignableTo(typeof(TimerViewModel)))
        {
            if (((TimerViewModel)_currentBindingContext).IsTimerRunning)
            {
                _timeLeft = ((TimerViewModel)_currentBindingContext).CountdownLabel;
            }
        }

        base.DidEnterBackground(application);
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
        DateTime awakeTime = DateTime.UtcNow;
        TimeSpan elapsedTime = DateTime.UtcNow.Subtract(_backgroundedTime);

        if (_currentBindingContext.GetType().IsAssignableTo(typeof(TimerViewModel)))
        {
            if (((TimerViewModel)_currentBindingContext).IsTimerRunning)
            {
                if (elapsedTime < _timeLeft)
                {
                    ((TimerViewModel)_currentBindingContext).CountdownLabel = _timeLeft.Subtract(elapsedTime);
                }
                else
                {
                    ((TimerViewModel)_currentBindingContext).SelectedTea = null;
                    ((TimerViewModel)_currentBindingContext).IsButtonEnabled = false;
                }
            }
        }
        base.WillEnterForeground(application);
    }

    /// <inheritdoc cref="MauiUIApplicationDelegate.WillFinishLaunching(UIApplication, NSDictionary)"/>
    public override bool WillFinishLaunching(UIApplication application, NSDictionary launchOptions)
    {
        try
        {
            UNUserNotificationCenter.Current.Delegate = new NotificationCenterDelegate();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return base.WillFinishLaunching(application, launchOptions);
    }
}

