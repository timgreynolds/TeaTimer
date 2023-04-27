
using System;
using AudioToolbox;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    public partial class TimerViewModel
    {
        partial void TimerExpired()
        {
            string soundFileName = @"/System/Library/PrivateFrameworks/ToneLibrary.framework/Versions/A/Resources/AlertTones/alarm.caf";
            if (Uri.TryCreate(soundFileName, UriKind.Absolute, out Uri uri))
            {
                //SystemSound alarmSound = new SystemSound(uri).PlaySystemSound();
                new SystemSound(uri).PlaySystemSound();
            }
        }
    }
}

