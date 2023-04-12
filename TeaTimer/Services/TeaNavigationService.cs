using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public class TeaNavigationService : INavigationService
    {
        public TeaNavigationService()
        {
        }

        public Task InitializeAsync() => NavigateToAsync(nameof(Pages.TimerPage));

        public Task GoBackAsync(bool animate = true) => AppShell.Current.GoToAsync("..", animate);

        public Task NavigateToAsync(string route, IDictionary<string, object> routeParameters = null)
        {
            ShellNavigationState state = new ShellNavigationState(route);
            return (routeParameters is null) ? AppShell.Current.GoToAsync(state) : AppShell.Current.GoToAsync(state, routeParameters);
        }

        public event EventHandler<EventArgs> NavigatedTo;

        public event EventHandler<EventArgs> NavigatedFrom;

        public event EventHandler<EventArgs> NavigatingFrom;

        public event EventHandler<EventArgs> ShellNavigating
        {
            add => AppShell.Current.Navigated += (sender, args) => value(AppShell.Current, args);
            remove => AppShell.Current.Navigated -= (sender, args) => value(AppShell.Current, args);
        }

        public event EventHandler<EventArgs> ShellNavigated
        {
            add => AppShell.Current.Navigated += (sender, args) => value(AppShell.Current, args);
            remove => AppShell.Current.Navigated -= (sender, args) => value(AppShell.Current, args);
        }
    }
}