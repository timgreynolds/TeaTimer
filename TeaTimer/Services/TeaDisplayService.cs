using System.Threading.Tasks;
using Microsoft.Maui;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    /// <inheritdoc cref="Services.IDisplayService" />
    public class TeaDisplayService : IDisplayService
    {
        /// <inheritdoc cref="Microsoft.Maui.Controls.Page.DisplayAlert(string, string, string)"/>
        public Task ShowAlertAsync(string title, string message, string cancel = "Cancel") => AppShell.Current.DisplayAlert(title, message, cancel, FlowDirection.MatchParent);

        /// <inheritdoc cref="Microsoft.Maui.Controls.Page.DisplayAlert(string, string, string, string)"/>
        public Task<bool> ShowPromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel") => AppShell.Current.DisplayAlert(title, message, accept, cancel, FlowDirection.MatchParent);

        /// <summary>
        /// Generic Exception Handling alert. Displays a pop-up with the Exception type, Exception message, and an OK button.
        /// </summary>
        /// <param name="ex">The Exception thrown.</param>
        public Task ShowExceptionAsync(System.Exception ex) => AppShell.Current.DisplayAlert(ex.GetType().Name, ex.Message, "OK", FlowDirection.MatchParent);

        /// <inheritdoc cref="Microsoft.Maui.Controls.Page.ForceLayout()"/>
        public void RefreshView() => AppShell.Current.CurrentPage.ForceLayout();

        /// <inheritdoc cref="Microsoft.Maui.Controls.Page.IsBusy"/>
        public void SetIsBusy(bool busy)
        {
            AppShell.Current.CurrentPage.IsBusy = busy;
        }
    }
}

