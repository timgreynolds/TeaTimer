using com.mahonkin.tim.maui.TeaTimer.Platforms.iOS;
using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using UIKit;
using UserNotifications;

namespace com.mahonkin.tim.maui.TeaTimer;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

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

