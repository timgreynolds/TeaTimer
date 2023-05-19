using System;
using System.Threading.Tasks;
using Microsoft.Maui.Dispatching;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public class TeaTimerService : ITimerService
    {
        private IDispatcherTimer _countdown = null;

        public TeaTimerService()
        {
        }

        public TimeSpan Interval
        {
            get => _countdown.Interval;
            set => _countdown.Interval = value;
        }

        public bool IsRepeating
        {
            get => _countdown.IsRepeating;
            set => _countdown.IsRepeating = value;
        }

        public bool IsRunning => _countdown.IsRunning;

        public event EventHandler Tick
        {
            add => _countdown.Tick += value;
            remove => _countdown.Tick -= value;
        }

        public void CreateTimer()
        {
            if(_countdown is null)
            {
                _countdown = AppShell.Current.Dispatcher.CreateTimer();
            }
        }

        public void Start()
        {
            _countdown.Start();
        }

        public void Start(TimeSpan duration)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            _countdown.Stop();
        }
    }
}

