using System;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public interface ITimerService
    {
        public TimeSpan Interval { get; set; }
        public bool IsRepeating { get; set; }
        public bool IsRunning { get; }
        public event EventHandler Tick;
        public void Start();
        public void Stop();
        public object CreateTimer();
    }
}

