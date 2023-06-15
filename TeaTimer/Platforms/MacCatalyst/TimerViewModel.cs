using System;
using System.Threading.Tasks;
using AudioToolbox;
using UserNotifications;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    /// <inheritdoc cref="BaseViewModel"/>
    public partial class TimerViewModel
    {
        async private partial Task TimerExpired()
        {
            UNNotificationSettings settings = await UNUserNotificationCenter.Current.GetNotificationSettingsAsync();
            if (settings.AuthorizationStatus != UNAuthorizationStatus.Authorized)
            {
                string soundFileName = @"/System/Library/PrivateFrameworks/ToneLibrary.framework/Versions/A/Resources/AlertTones/alarm.caf";
                if (Uri.TryCreate(soundFileName, UriKind.Absolute, out Uri uri))
                {
                    await new SystemSound(uri).PlaySystemSoundAsync();
                }
            }
        }
    }
}

