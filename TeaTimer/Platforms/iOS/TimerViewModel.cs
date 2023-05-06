using System.Threading.Tasks;
using AudioToolbox;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    public partial class TimerViewModel
    {
        async private partial Task TimerExpired()
        {
            SystemSound alarmSound = new SystemSound(1005);
            await alarmSound.PlaySystemSoundAsync();
        }
    }
}

