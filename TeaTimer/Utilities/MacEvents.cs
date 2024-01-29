using System;
using com.mahonkin.tim.maui.TeaTimer.Services;
using com.mahonkin.tim.maui.TeaTimer.ViewModels;
using Foundation;
using Microsoft.Maui.Controls;
using UIKit;
using UserNotifications;

namespace com.mahonkin.tim.maui.TeaTimer.Utilities
{
    public static class MacEvents
    {
        internal static async void OnActivatedEvent(UIApplication application)
        {
            UNNotificationSettings settings = await UNUserNotificationCenter.Current.GetNotificationSettingsAsync();
            switch (settings.AuthorizationStatus)
            {
                case UNAuthorizationStatus.Authorized:
                    break;
                case UNAuthorizationStatus.Denied:
                    DisplayAlert(UNAuthorizationStatus.Denied);
                    break;
                case UNAuthorizationStatus.NotDetermined:
                    UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Sound | UNAuthorizationOptions.Alert, ProcessAuthRequest);
                    break;
                case UNAuthorizationStatus.Provisional:
                    DisplayAlert(UNAuthorizationStatus.Provisional);
                    break;
                case UNAuthorizationStatus.Ephemeral:
                    DisplayAlert(UNAuthorizationStatus.Provisional);
                    break;
                default:
                    break;
            }
        }

        private static void ProcessAuthRequest(bool auth, NSError error)
        {
            if (error is not null)
            {
                DisplayError(error);
            }
        }

        private static async void DisplayError(NSError error)
        {
            IDisplayService displayService = GetPageDisplayService();
            await displayService.ShowAlertAsync("Error!", $"An error occurred requesting Notification Authorization.\n{error.LocalizedDescription}");
        }

        private static IDisplayService GetPageDisplayService()
        {
            IDisplayService displayService = null;
            Page currentPage = AppShell.Current.CurrentPage ?? (Page)AppShell.Current;
            if (currentPage.BindingContext.GetType().IsAssignableTo(typeof(BaseViewModel)))
            {
                BaseViewModel bindingContext = currentPage.BindingContext as BaseViewModel;
                displayService = bindingContext.DisplayService;
            }
            return displayService;
        }

        private static async void DisplayAlert(UNAuthorizationStatus status)
        {
            IDisplayService displayService = GetPageDisplayService();
            switch (status)
            {
                case UNAuthorizationStatus.Denied:
                    await displayService.ShowAlertAsync("Warning!", 
                            "Notifications are not enabled for Tea Timer. You will not receive an alert when the timer expires unless the application is in the foreground. Please enable Notifications in the Settings app.", "OK");
                    break;
                default:
                    await displayService.ShowAlertAsync("Note!", 
                            "Notifications for Tea Timer have been granted as 'Provisional' or 'Ephemeral'. They may work now, but Notifications should be enabled in the Settings app.", "OK");
                    break;
            }
        }
    }
}