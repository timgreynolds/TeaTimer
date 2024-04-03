using System;
using System.Threading.Tasks;
using AudioToolbox;
using com.mahonkin.tim.maui.TeaTimer.Utilities;

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
        private partial async Task TimerExpired()
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
                    throw new Exception(ex.Message, ex);
                }
            }
        }
    }
}

