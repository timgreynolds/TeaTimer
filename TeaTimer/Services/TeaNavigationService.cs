using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    /// <inheritdoc cref="Services.INavigationService"/>
    public class TeaNavigationService : INavigationService
    {
        /// <inheritdoc cref="INavigationService.InitializeAsync()" />
        public Task InitializeAsync() => NavigateToAsync(nameof(Pages.TimerPage));

        /// <inheritdoc cref="INavigationService.GoBackAsync(bool)" />
        public Task GoBackAsync(bool animate = true) => AppShell.Current.GoToAsync("..", animate);

        /// <inheritdoc cref="INavigationService.NavigateToAsync(string, IDictionary{string, object})" />
        public Task NavigateToAsync(string route, IDictionary<string, object> routeParameters = null)
        {
            ShellNavigationState state = new ShellNavigationState(route);
            return (routeParameters is null) ? AppShell.Current.GoToAsync(state) : AppShell.Current.GoToAsync(state, routeParameters);
        }

        /// <inheritdoc cref="INavigationService.NavigatedTo" />
        public event EventHandler<EventArgs> NavigatedTo
        {
            add => AppShell.Current.CurrentPage.NavigatedTo += (sender, args) => value(AppShell.Current, args);
            remove => AppShell.Current.CurrentPage.NavigatedTo -= (sender, args) => value(AppShell.Current, args);
        }

        /// <inheritdoc cref="INavigationService.NavigatedFrom" />
        public event EventHandler<EventArgs> NavigatedFrom
        {
            add => AppShell.Current.CurrentPage.NavigatedFrom += (sender, args) => value(AppShell.Current, args);
            remove => AppShell.Current.CurrentPage.NavigatedFrom -= (sender, args) => value(AppShell.Current, args);
        }

        /// <inheritdoc cref="INavigationService.NavigatingFrom" />
        public event EventHandler<EventArgs> NavigatingFrom
        {
            add => AppShell.Current.CurrentPage.NavigatingFrom += (sender, args) => value(AppShell.Current, args);
            remove => AppShell.Current.CurrentPage.NavigatingFrom -= (sender, args) => value(AppShell.Current, args);
        }

        /// <inheritdoc cref="INavigationService.ShellNavigated" />
        public event EventHandler<EventArgs> ShellNavigated
        {
            add => AppShell.Current.Navigated += (sender, args) => value(AppShell.Current, args);
            remove => AppShell.Current.Navigated -= (sender, args) => value(AppShell.Current, args);
        }

        /// <inheritdoc cref="INavigationService.ShellNavigating" />
        public event EventHandler<EventArgs> ShellNavigating
        {
            add => AppShell.Current.Navigating += (sender, args) => value(AppShell.Current, args);
            remove => AppShell.Current.Navigating -= (sender, args) => value(AppShell.Current, args);
        }
    }
}