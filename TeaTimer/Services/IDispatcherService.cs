using System.Threading.Tasks;
using Microsoft.Maui.Dispatching;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public interface IDispatcherService
	{
		IDispatcher CreateDispatcher();
    }
}

