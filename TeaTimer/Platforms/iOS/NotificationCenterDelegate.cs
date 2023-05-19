using com.mahonkin.tim.maui.TeaTimer.Platforms.iOS;
using System;
using UserNotifications;

namespace com.mahonkin.tim.maui.TeaTimer.Platforms.iOS
{
    public class NotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        public NotificationCenterDelegate()
        {
        }

        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
        }

        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action ResponseHandler)
        {
        }
    }
}

