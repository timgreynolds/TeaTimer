using System.Threading.Tasks;
using Microsoft.Maui;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public interface IDisplayService
    {
        Task ShowExceptionAsync(System.Exception exception);

        Task ShowAlertAsync(string title, string message, string cancel = "Cancel", FlowDirection flowDirection = FlowDirection.MatchParent);

        Task<bool> ShowPromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel", FlowDirection flowDirection = FlowDirection.MatchParent);
    }	
}

