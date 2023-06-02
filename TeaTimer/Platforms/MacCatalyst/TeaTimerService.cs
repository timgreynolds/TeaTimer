using System;
using Microsoft.Maui.Dispatching;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    /// <inheritdoc cref="ITimerService"/>
    public class TeaTimerService : ITimerService
    {
        private IDispatcherTimer _countdown = null;

        /// <inheritdoc cref="ITimerService.Interval" />
        public TimeSpan Interval
        {
            get => _countdown.Interval;
            set => _countdown.Interval = value;
        }

        /// <inheritdoc cref="ITimerService.IsRepeating" />
        public bool IsRepeating
        {
            get => _countdown.IsRepeating;
            set => _countdown.IsRepeating = value;
        }

        /// <inheritdoc cref="ITimerService.IsRunning" />
        public bool IsRunning => _countdown.IsRunning;

        /// <inheritdoc cref="ITimerService.Tick" />
        public event EventHandler Tick
        {
            add => _countdown.Tick += value;
            remove => _countdown.Tick -= value;
        }

        /// <inheritdoc cref="ITimerService.CreateTimer()" />
        public void CreateTimer()
        {
            if(_countdown is null)
            {
                _countdown = AppShell.Current.Dispatcher.CreateTimer();
            }
        }

        /// <inheritdoc cref="ITimerService.Start()" />
        public void Start()
        {
            _countdown.Start();
        }

        /// <inheritdoc cref="ITimerService.Start(TimeSpan)" />
        public void Start(TimeSpan duration)
        {
            Start();
        }

        /// <inheritdoc cref="ITimerService.Stop()" />
        public void Stop()
        {
            _countdown.Stop();
        }
    }
}

