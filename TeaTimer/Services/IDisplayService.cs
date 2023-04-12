using System.Threading.Tasks;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public interface IDisplayService
    {
        Task ShowExceptionAsync(System.Exception exception);

        Task ShowAlertAsync(string title, string message, string cancel = "Cancel");

        Task<bool> ShowPromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel");

        void RefreshView();

        void SetIsBusy(bool busy = true);
    }	
}

