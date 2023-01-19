using System.Threading.Tasks;
using Microsoft.Maui;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public class DisplayService : IDisplayService
	{
		public DisplayService()
		{
		}

		/// <inheritdoc cref="Microsoft.Maui.Controls.Page.DisplayAlert(string, string, string, FlowDirection)"/>
		public Task ShowAlertAsync(string title, string message, string cancel = "Cancel", FlowDirection flowDirection = FlowDirection.MatchParent)
		{
			return AppShell.Current.DisplayAlert(title, message, cancel, flowDirection);
		}

        /// <inheritdoc cref="Microsoft.Maui.Controls.Page.DisplayAlert(string, string, string, string, FlowDirection)"/>
        public Task<bool> ShowPromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel", FlowDirection flowDirection = FlowDirection.MatchParent)
		{
			return AppShell.Current.DisplayAlert(title, message, accept, cancel, flowDirection);
        }

		/// <summary>
		/// Generic Exception Handling alert. Displays a pop-up with the Exception type, Exception message, and an OK button.
		/// </summary>
		/// <param name="ex">The Exception thrown.</param>
        public Task ShowExceptionAsync(System.Exception ex)
		{
			return  AppShell.Current.DisplayAlert(ex.GetType().Name, ex.Message, "OK", FlowDirection.MatchParent);
		}
    }
}

