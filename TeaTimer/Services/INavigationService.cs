using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    /// <summary>
    /// Represents an interface that can be used to provide some simple
    /// navigation options.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// If any initialization, setting MainPage for example, do it here.
        /// </summary>
        public Task InitializeAsync();

        /// <summary>
        /// Navigate to the page specified by <paramref name="route">route
        /// </paramref> and pass a list of parameters to the new page using
        /// <paramref name="routeParameters">routeParameters</paramref>
        /// </summary>
        /// <param name="route"/>
        /// <param name="routeParameters"/>
        public Task NavigateToAsync(string route, IDictionary<string, object> routeParameters = null);

        /// <summary>
        /// Navigate to the previous page.
        /// </summary>
        public Task GoBackAsync(bool animate = true);

        /// <summary>
        /// Event that is fired when navigation to the page is complete. 
        /// </summary>
        public event EventHandler<EventArgs> NavigatedTo;

        /// <summary>
        /// Event that is fired when navigation away from the page is complete.
        /// </summary> 
        public event EventHandler<EventArgs> NavigatedFrom;

        /// <summary>
        /// Event that is fired when navigation away from the page is about to
        /// be performed.
        /// </summary>
        public event EventHandler<EventArgs> NavigatingFrom;

        /// <summary>
        /// Event that is fired when shell navigation has completed.
        /// </summary>
        public event EventHandler<EventArgs> ShellNavigated;

        /// <summary>
        /// Event that is fired when shell navigation is about to be performed.
        /// </summary>
        public event EventHandler<EventArgs> ShellNavigating;
    }

}

