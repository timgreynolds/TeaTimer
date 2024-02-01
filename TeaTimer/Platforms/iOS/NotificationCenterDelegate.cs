using System;
using UserNotifications;

namespace com.mahonkin.tim.maui.TeaTimer.Platforms.iOS
{
    /// <inheritdoc cref="UNUserNotificationCenterDelegate" />
    public class NotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        /// <inheritdoc cref="NotificationCenterDelegate()" /> 
        public NotificationCenterDelegate()
        {
        }

        /// <inheritdoc cref="WillPresentNotification(UNUserNotificationCenter, UNNotification, Action{UNNotificationPresentationOptions})" />
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            //TODO: Probably should do something here to check if the TimerService countdown is active and cancel if it is.
            completionHandler(UNNotificationPresentationOptions.Banner);
        }

        /// <inheritdoc cref="DidReceiveNotificationResponse(UNUserNotificationCenter, UNNotificationResponse, Action)"/>
        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action responseHandler)
        {
            responseHandler();
        }
    }
}

