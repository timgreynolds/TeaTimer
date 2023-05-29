using System;
using System.Reflection;
using com.mahonkin.tim.maui.TeaTimer.Platforms.iOS;
using com.mahonkin.tim.maui.TeaTimer.ViewModels;
using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using UIKit;
using UserNotifications;

namespace com.mahonkin.tim.maui.TeaTimer;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    private TimeSpan _timeLeft = TimeSpan.Zero;
    private DateTime _backgroundedTime = DateTime.UtcNow;
    private object _currentBindingContext;
    private CoreFoundation.OSLog _logger = new CoreFoundation.OSLog(Assembly.GetExecutingAssembly().GetName().Name, nameof(AppDelegate));

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override void DidEnterBackground(UIApplication application)
    {
        _logger.Log(CoreFoundation.OSLogLevel.Info, $"====Entering background====");
        _logger.Log(CoreFoundation.OSLogLevel.Info, $"====Countdown left: {_timeLeft.ToString(@"mm\:ss")}");
        _logger.Log(CoreFoundation.OSLogLevel.Info, $"====Backgrounded at: {_backgroundedTime.ToLocalTime().ToString(@"hh\:mm\:ss")}");
        _currentBindingContext = AppShell.Current.CurrentPage.BindingContext;
        _logger.Log($"====Page viewmodel type: {_currentBindingContext.GetType().Name}");
        if (_currentBindingContext.GetType().IsAssignableTo(typeof(TimerViewModel)))
        {
            _logger.Log($"====Saving state====");
            _timeLeft = ((TimerViewModel)_currentBindingContext).CountdownLabel;
            _backgroundedTime = DateTime.UtcNow;
            _logger.Log($"====Countdown left: {_timeLeft.ToString(@"mm\:ss")}");
            _logger.Log($"====Backgrounded at: {_backgroundedTime.ToLocalTime().ToString(@"hh\:mm\:ss")}");
        }
        base.DidEnterBackground(application);
        _logger.Log($"====Entered background=====");
    }

    public override void WillEnterForeground(UIApplication application)
    {
        _logger.Log($"====Entering foreground====");
        _logger.Log($"====Countdown left when backgrounded: {_timeLeft.ToString(@"mm\:ss")}");
        _logger.Log($"====Backgrounded at: {_backgroundedTime.ToLocalTime().ToString(@"hh\:mm\:ss")}");
        _logger.Log($"====Awoken at: {DateTime.UtcNow.ToLocalTime().ToString(@"hh\:mm\:ss")}");
        TimeSpan elapsedTime = DateTime.UtcNow.Subtract(_backgroundedTime);
        _logger.Log($"====Time spent in background: {elapsedTime.ToString(@"mm\:ss")}");
        TimeSpan remainingTime = _timeLeft.Subtract(elapsedTime);
        _logger.Log($"====Page viewmodel type: {_currentBindingContext.GetType().Name}");
        if (_currentBindingContext.GetType().IsAssignableTo(typeof(TimerViewModel)))
        {
            _logger.Log($"====Resetting state====");
            if (remainingTime > TimeSpan.Zero)
            {
                _logger.Log($"====Current time remaining: {remainingTime.ToString(@"mm\:ss")}");
                ((TimerViewModel)_currentBindingContext).CountdownLabel = remainingTime;
                _logger.Log($"====Page Countdown set to: {remainingTime.ToString(@"mm\:ss")}");
            }
            else
            {
                _logger.Log($"====Timer already expired====");
                ((TimerViewModel)_currentBindingContext).CountdownLabel = TimeSpan.Zero;
            }
            _logger.Log($"====State reset====");
        }
        base.WillEnterForeground(application);
        _logger.Log($"====Entered foreground====");
    }

    public override bool WillFinishLaunching(UIApplication application, NSDictionary launchOptions)
    {
        try
        {
            UNUserNotificationCenter.Current.Delegate = new NotificationCenterDelegate();
        }
        catch (Exception ex)
        {
            _logger.Log(ex.Message);
        }
        return base.WillFinishLaunching(application, launchOptions);
    }
}

