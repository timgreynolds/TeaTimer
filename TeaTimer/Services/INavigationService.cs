using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public interface INavigationService
    {
        public Task InitializeAsync();
        public Task NavigateToAsync(string route, IDictionary<string, object> routeParameters);
        public Task GoBackAsync(bool animate);
        public event EventHandler<EventArgs> NavigatedTo;
        public event EventHandler<EventArgs> NavigatedFrom;
        public event EventHandler<EventArgs> NavigatingFrom;
        public event EventHandler<EventArgs> ShellNavigating;
        public event EventHandler<EventArgs> ShellNavigated;
    }

}

