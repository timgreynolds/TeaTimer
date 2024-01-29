using System;
using AudioToolbox;
using Microsoft.Extensions.Logging;
using com.mahonkin.tim.maui.TeaTimer.Utilities;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    /// <inheritdoc cref="BaseViewModel"/>
    public partial class TimerViewModel
    {
        private async partial void TimerExpired()
        {
            string soundFileName = FileSystemUtils.GetAppDataFileFullName("kettle.mp3");

            if (Uri.TryCreate(soundFileName, UriKind.Absolute, out Uri uri))
            {
                try
                {
                    using (SystemSound sound = new SystemSound(uri))
                    {
                        await sound.PlayAlertSoundAsync();
                    }
                }
                catch (Exception ex)
                {
                    MauiProgram.Logger.LogCritical("An error occurred {Message}", ex.Message);
                }
            }
        }
    }
}

