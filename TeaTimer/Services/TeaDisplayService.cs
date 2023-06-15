using System.Threading.Tasks;
using Microsoft.Maui;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    /// <summary>
    /// Implementation of <see cref="IDisplayService">IDisplayService</see> using
    /// Maui Controls.
    /// </summary>
    public class TeaDisplayService : IDisplayService
    {
        /// <summary>
        /// Display an alert with a single button.
        /// </summary>
        /// <param name="title">The title of the displayed alert.</param>
        /// <param name="message">The descriptive text of the alert.</param>
        /// <param name="cancel">The text to be displayed on the button.</param>
        public Task ShowAlertAsync(string title, string message, string cancel = "Cancel")
            => AppShell.Current.DisplayAlert(title, message, cancel, FlowDirection.MatchParent);

        /// <summary>
        /// Display an alert with two buttons that represent an "accept" and
        /// "cancel" intent.
        /// </summary>
        /// <param name="title">The title of the displayed alert.</param>
        /// <param name="message">The descriptive text of the alert. Should
        /// describe the consequences of selecting either button.</param>
        /// <param name="accept">The text to be displayed on the 'accept' button.
        /// Default is "OK".</param>
        /// <param name="cancel">The text top be displayed on the 'cancel' button.
        /// Default is "Cancel".</param>
        /// <returns>A Task representing the result of the alert. The task result
        /// contains true if the 'accept' button was selected and false otherwise.
        /// </returns>
        public Task<bool> ShowPromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel")
            => AppShell.Current.DisplayAlert(title, message, accept, cancel, FlowDirection.MatchParent);

        /// <summary>
        /// Generic Exception Handling alert. Displays a pop-up with the Exception
        /// type, Exception message, and an "OK" button.
        /// </summary>
        /// <param name="ex">The Exception that was thrown.</param>
        public Task ShowExceptionAsync(System.Exception ex)
            => AppShell.Current.DisplayAlert(ex.GetType().Name, ex.Message, "OK", FlowDirection.MatchParent);

        /// <inheritdoc cref="Microsoft.Maui.Controls.Page.ForceLayout()"/>
        public void RefreshView() => AppShell.Current.CurrentPage.ForceLayout();

        /// <inheritdoc cref="Microsoft.Maui.Controls.Page.IsBusy"/>
        public void SetIsBusy(bool busy)
        {
            AppShell.Current.CurrentPage.IsBusy = busy;
        }
    }
}

