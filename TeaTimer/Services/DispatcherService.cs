using Microsoft.Maui.Dispatching;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public class DispatcherService : IDispatcherService
	{
		public DispatcherService()
		{
		}

		public IDispatcherTimer CreateTimer()
		{
			return AppShell.Current.Dispatcher.CreateTimer();
		}

		public bool Dispatch(System.Action action)
		{
			return AppShell.Current.Dispatcher.Dispatch(action);
		}
	}
}

