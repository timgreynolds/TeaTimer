using System;
using Microsoft.Maui.Dispatching;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    /// <summary>
    /// Implementation of <see cref="ITimerService"/> that depends on Maui dispatchers.
    /// </summary>
    public class TeaTimerService : ITimerService
    {
        #region Private Fields
        private IDispatcherTimer _countdown = null;
        #endregion Private Fields


        #region Public Properties
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
        #endregion Public Properties

        #region Public Methods
        /// <summary>
        /// Creates an instance of an IDispatcherTimer associated with the
        /// current shell's Dispatcher.
        /// </summary>
        /// <remarks>
        /// This works for MacCatalyst, I assume, because the shell's Dispatcher
        /// continues to run on the main/UI thread when the app is backgrounded,
        /// minimized, or hidden.
        /// </remarks>
        public void CreateTimer()
        {
            _countdown ??= AppShell.Current.Dispatcher.CreateTimer();
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            _countdown.Start();
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <remarks>
        /// This method is provided for compatibility only. The <paramref
        /// name="duration">duration</paramref> is ignored and <see
        /// cref="Start()"/> is called.
        /// </remarks>
        public void Start(TimeSpan duration)
        {
            Start();
        }

        /// <inheritdoc cref="ITimerService.Stop()" />
        public void Stop()
        {
            _countdown.Stop();
        }
        #endregion Public Methods
    }
}