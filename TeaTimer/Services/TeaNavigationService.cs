using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    /// <summary>
    /// Implementation of <see cref="INavigationService">INavigationService</see>
    /// using Maui Shell Navigation.
    /// </summary>
    public class TeaNavigationService : INavigationService
    {
        /// <summary>
        /// Navigates to the Main/Timer Page in an asynchronous manner.
        /// </summary>
        public Task InitializeAsync() => NavigateToAsync(nameof(Pages.TimerPage));

        /// <summary>
        /// Navigates back to previous page in an asynchronous manner, optionally
        /// animating the transition.
        /// </summary>
        /// <param name="animate">
        /// Whether or not to animate the transition. Default is true.
        /// </param>
        public Task GoBackAsync(bool animate = true) => AppShell.Current.GoToAsync("..", animate);

        /// <summary>
        /// Navigate to the specified <paramref name="route" /> in an
        /// asynchronous manner optionally passing <paramref name="routeParameters" />
        /// parameters.
        /// </summary>
        public Task NavigateToAsync(string route, IDictionary<string, object> routeParameters = null)
        {
            ShellNavigationState state = new ShellNavigationState(route);
            return (routeParameters is null) ? AppShell.Current.GoToAsync(state) : AppShell.Current.GoToAsync(state, routeParameters);
        }

        /// <inheritdoc cref="INavigationService.NavigatedTo" />
        /// <remarks>
        /// Use the Shell Navigation event handlers if possible. <br/>
        /// EventArgs is an instance of <see cref="NavigatedToEventArgs"/>.
        /// </remarks>
        public event EventHandler<EventArgs> NavigatedTo
        {
            add => AppShell.Current.CurrentPage.NavigatedTo += (sender, args) => value(AppShell.Current, args);
            remove => AppShell.Current.CurrentPage.NavigatedTo -= (sender, args) => value(AppShell.Current, args);
        }

        /// <inheritdoc cref="INavigationService.NavigatedFrom" />
        /// <remarks>
        /// Use the Shell Navigation event handlers if possible. <br/>
        /// EventArgs is an instance of <see cref="NavigatedFromEventArgs"/>.
        /// </remarks>
        public event EventHandler<EventArgs> NavigatedFrom
        {
            add => AppShell.Current.CurrentPage.NavigatedFrom += (sender, args) => value(AppShell.Current, args);
            remove => AppShell.Current.CurrentPage.NavigatedFrom -= (sender, args) => value(AppShell.Current, args);
        }

        /// <inheritdoc cref="INavigationService.NavigatingFrom" />
        /// <remarks>
        /// Use the Shell Navigation event handlers if possible. <br/>
        /// EventArgs is an instance of <see cref="NavigatingFromEventArgs"/>.
        /// </remarks>
        public event EventHandler<EventArgs> NavigatingFrom
        {
            add => AppShell.Current.CurrentPage.NavigatingFrom += (sender, args) => value(AppShell.Current, args);
            remove => AppShell.Current.CurrentPage.NavigatingFrom -= (sender, args) => value(AppShell.Current, args);
        }

        /// <summary>
        /// Event that is fired when shell navigation has completed.
        /// </summary>
        /// <remarks>
        /// EventArgs is an instance of <see cref="ShellNavigatedEventArgs" />.
        /// </remarks>
        public event EventHandler<EventArgs> ShellNavigated
        {
            add => AppShell.Current.Navigated += (sender, args) => value(AppShell.Current, args);
            remove => AppShell.Current.Navigated -= (sender, args) => value(AppShell.Current, args);
        }

        /// <summary>
        /// Event that is fired when shell navigation is about to be performed.
        /// </summary>
        /// <remarks>
        /// EventArgs is an instance of <see cref="ShellNavigatingEventArgs"/>.
        /// </remarks>
        public event EventHandler<EventArgs> ShellNavigating
        {
            add => AppShell.Current.Navigating += (sender, args) => value(AppShell.Current, args);
            remove => AppShell.Current.Navigating -= (sender, args) => value(AppShell.Current, args);
        }
    }
}