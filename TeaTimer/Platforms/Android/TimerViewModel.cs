using Android.Media;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    public partial class TimerViewModel
	{
        private partial void TimerExpired()
		{
			Android.Net.Uri alarmSoundUri = RingtoneManager.GetActualDefaultRingtoneUri(null, RingtoneType.Alarm);
			Ringtone alarmSound = RingtoneManager.GetRingtone(null, alarmSoundUri);
			alarmSound.Play();
		}
	}
}