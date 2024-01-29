using System.Threading.Tasks;
using AudioToolbox;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    /// <inheritdoc cref="ViewModels.TimerViewModel" />
    public partial class TimerViewModel
    {
        /// <summary>
        /// Whether the page's timer is currently running.
        /// </summary>
        public bool IsTimerRunning
        {
            get => _timerService.IsRunning;
        }

        /// <summary>
        /// Action to perform when the timer expires.
        /// </summary>
        private partial void TimerExpired()
        {
            SystemSound alarmSound = new SystemSound(1005);
            alarmSound.PlaySystemSound();
        }
    }
}

