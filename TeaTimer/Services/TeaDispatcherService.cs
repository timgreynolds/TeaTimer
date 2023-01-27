using System.Threading.Tasks;
using Microsoft.Maui.Dispatching;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public class TeaDispatcherService : IDispatcherService
    {
        public TeaDispatcherService()
        {
        }

        public IDispatcher CreateDispatcher() => AppShell.Current.Dispatcher;
    }
}

