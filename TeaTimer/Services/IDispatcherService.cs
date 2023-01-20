using Microsoft.Maui.Dispatching;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public interface IDispatcherService
	{
		IDispatcherTimer CreateTimer();

		bool Dispatch(System.Action action);
    }
}

