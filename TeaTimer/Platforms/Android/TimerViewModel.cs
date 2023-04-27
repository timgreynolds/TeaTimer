using Android.Content;
using Android.Media;
using Uri = Android.Net.Uri;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
	public partial class TimerViewModel
	{
        partial void TimerExpired()
		{
			Uri alarmSoundUri = RingtoneManager.GetActualDefaultRingtoneUri(null, RingtoneType.Alarm);
			Ringtone alarmSound = RingtoneManager.GetRingtone(null, alarmSoundUri);
			alarmSound.Play();
		}
	}
}