using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
	public interface INavigationService
	{
		Task InitializeAsync();

		Task NavigateToAsync(string route, IDictionary<string, object> routeParameters = null);

		Task PopAsync();

		Task GoBackAsync();
	}
}

