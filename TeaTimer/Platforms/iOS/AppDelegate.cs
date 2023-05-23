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
        Console.WriteLine($"====Countdown left: {_timeLeft.ToString(@"mm\:ss")}");
        Console.WriteLine($"====Backgrounded at: {_backgroundedTime.ToLocalTime().ToString(@"hh\:mm\:ss")}");
        _currentBindingContext = AppShell.Current.CurrentPage.BindingContext;
        Console.WriteLine($"====Page viewmodel type: {_currentBindingContext.GetType().Name}");
        if (_currentBindingContext.GetType().IsAssignableTo(typeof(TimerViewModel)))
        {
            Console.WriteLine($"====Saving state====");
            _timeLeft = ((TimerViewModel)_currentBindingContext).CountdownLabel;
            _backgroundedTime = DateTime.UtcNow;
            Console.WriteLine($"====Countdown left: {_timeLeft.ToString(@"mm\:ss")}");
            Console.WriteLine($"====Backgrounded at: {_backgroundedTime.ToLocalTime().ToString(@"hh\:mm\:ss")}");
        }
        base.DidEnterBackground(application);
        Console.WriteLine($"====Entered background=====");
    }

    public override void WillEnterForeground(UIApplication application)
    {
        Console.WriteLine($"====Entering foreground====");
        Console.WriteLine($"====Countdown left when backgrounded: {_timeLeft.ToString(@"mm\:ss")}");
        Console.WriteLine($"====Backgrounded at: {_backgroundedTime.ToLocalTime().ToString(@"hh\:mm\:ss")}");
        Console.WriteLine($"====Awoken at: {DateTime.UtcNow.ToLocalTime().ToString(@"hh\:mm\:ss")}");
        TimeSpan elapsedTime = DateTime.UtcNow.Subtract(_backgroundedTime);
        Console.WriteLine($"====Time spent in background: {elapsedTime.ToString(@"mm\:ss")}");
        TimeSpan remainingTime = _timeLeft.Subtract(elapsedTime);
        Console.WriteLine($"====Page viewmodel type: {_currentBindingContext.GetType().Name}");
        if (_currentBindingContext.GetType().IsAssignableTo(typeof(TimerViewModel)))
        {
            Console.WriteLine($"====Resetting state====");
            if (remainingTime > TimeSpan.Zero)
            {
                Console.WriteLine($"====Current time remaining: {remainingTime.ToString(@"mm\:ss")}");
                ((TimerViewModel)_currentBindingContext).CountdownLabel = remainingTime;
                Console.WriteLine($"====Page Countdown set to: {remainingTime.ToString(@"mm\:ss")}");
            }
            else
            {
                Console.WriteLine($"====Timer already expired====");
                ((TimerViewModel)_currentBindingContext).CountdownLabel = TimeSpan.Zero;
            }
            Console.WriteLine($"====State reset====");
        }
        base.WillEnterForeground(application);
        Console.WriteLine($"====Entered foreground====");
    }

    public override bool WillFinishLaunching(UIApplication application, NSDictionary launchOptions)
    {
        try
        {
            UNNotificationAction timerExpiredOkAction = UNNotificationAction.FromIdentifier(Constants.TIMER_EXPIRED_OK_ACTION, "OK", UNNotificationActionOptions.None, UNNotificationActionIcon.CreateFromSystem("timer"));
            UNNotificationAction timerExpiredCancelAction = UNNotificationAction.FromIdentifier(Constants.TIMER_EXPIRED_CANCEL_ACTION, "Cancel", UNNotificationActionOptions.None, UNNotificationActionIcon.CreateFromSystem("wrongwaysign"));
            UNNotificationCategory timerExpiredCategory = UNNotificationCategory.FromIdentifier(Constants.TIMER_EXPIRED_CATEGORY, new[] { timerExpiredOkAction, timerExpiredCancelAction }, new[] { string.Empty }, UNNotificationCategoryOptions.None);
            NSSet<UNNotificationCategory> categories = new NSSet<UNNotificationCategory>(new[] { timerExpiredCategory });
            UNUserNotificationCenter.Current.SetNotificationCategories(categories);
            UNUserNotificationCenter.Current.Delegate = new NotificationCenterDelegate();
        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine(ex.Message);
        }
        return base.WillFinishLaunching(application, launchOptions);
    }
}

