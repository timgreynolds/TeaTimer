using System;
using UserNotifications;

namespace com.mahonkin.tim.maui.TeaTimer.Platforms.iOS
{
    /// <inheritdoc cref="UNUserNotificationCenterDelegate" />
    public class NotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        /// <inheritdoc cref="NotificationCenterDelegate()" /> 
        //public NotificationCenterDelegate(object context) { }

        /// <inheritdoc cref="WillPresentNotification(UNUserNotificationCenter, UNNotification, Action{UNNotificationPresentationOptions})" />
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler) { }

        /// <inheritdoc cref="DidReceiveNotificationResponse(UNUserNotificationCenter, UNNotificationResponse, Action)"/>
        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action responseHandler) { }
    }
}
