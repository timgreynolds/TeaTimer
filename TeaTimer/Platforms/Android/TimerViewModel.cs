using System.Threading.Tasks;
using Android.Media;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    public partial class TimerViewModel
	{
        async private partial Task TimerExpired()
		{
			Android.Net.Uri alarmSoundUri = RingtoneManager.GetActualDefaultRingtoneUri(null, RingtoneType.Alarm);
			Ringtone alarmSound = RingtoneManager.GetRingtone(null, alarmSoundUri);
			await Task.Run(() => alarmSound.Play());
		}
	}
}