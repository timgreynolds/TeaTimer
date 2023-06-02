using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    /// <summary>
    /// Provides an interface that supports some simple navigation options.
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
        public Task NavigateToAsync(string route, IDictionary<string, object> routeParameters);

        /// <summary>
        /// Navigate to the previous page.
        /// </summary>
        public Task GoBackAsync(bool animate);

        /// <inheritdoc cref="Microsoft.Maui.Controls.Page.NavigatedTo" />
        public event EventHandler<EventArgs> NavigatedTo;

        /// <inheritdoc cref="Microsoft.Maui.Controls.Page.NavigatedTo" />
        public event EventHandler<EventArgs> NavigatedFrom;

        /// <inheritdoc cref="Microsoft.Maui.Controls.Page.NavigatingFrom" />
        public event EventHandler<EventArgs> NavigatingFrom;

        /// <inheritdoc cref="Microsoft.Maui.Controls.Shell.Navigated" />
        public event EventHandler<EventArgs> ShellNavigated;

        /// <inheritdoc cref="Microsoft.Maui.Controls.Shell.Navigating" />
        public event EventHandler<EventArgs> ShellNavigating;
    }

}

