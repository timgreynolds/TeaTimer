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

        public Task GoBackAsync(bool animate = true) => AppShell.Current.GoToAsync("..", animate);

        public Task InitializeAsync() => NavigateToAsync(nameof(Pages.TimerPage));

        public Task NavigateToAsync(string route, IDictionary<string, object> routeParameters = null)
        {
            ShellNavigationState state = new ShellNavigationState(route);
            return (routeParameters is null) ? AppShell.Current.GoToAsync(state) : AppShell.Current.GoToAsync(state, routeParameters);
        }
    }
}

