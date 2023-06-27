using System;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    /// <summary>
    /// Represents an interface that can be used to create platform-specific
    /// implementations of a timer or countdown. Use this along with a concrete
    /// implementation for each platform in your dependency injection container.
    /// </summary>
    public interface ITimerService
    {
        /// <summary>
        /// Gets or sets the amount of time between ticks.
        /// </summary>
        public TimeSpan Interval { get; set; }

        /// <summary>
        /// Gets or sets whether or not the timer should repeat. 
        /// </summary>
        public bool IsRepeating { get; set; }

        /// <summary>
        /// Gets the current running status of the timer.
        /// </summary>
        public bool IsRunning { get; }

        /// <summary>
        /// The action to perform when Interval has elapsed.
        /// </summary>
        public event EventHandler Tick;

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start();

        /// <summary>
        /// Starts the timer with the specified duration.
        /// </summary>
        /// <param name="duration">The amount of time thast the timer should run
        /// before auto-stopping.</param>
        public void Start(TimeSpan duration);

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop();

        /// <summary>
        /// Create the timer.
        /// </summary>
        public void CreateTimer();
    }
}