using AudioToolbox;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    public partial class TimerViewModel
	{
		private partial void TimerExpired()
		{
			SystemSound alarmSound = new SystemSound(1005);
			alarmSound.PlaySystemSound();
		}
	}
}

