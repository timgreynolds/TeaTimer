using System.Threading.Tasks;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    /// <summary>
    /// Represents and interface that can be used to provide platform-specific
    /// implementations of alert and error dialogs.
    /// </summary>
    public interface IDisplayService
    {
        /// <summary>
        /// Display an alert when an exception is thrown.
        /// </summary>
        Task ShowExceptionAsync(System.Exception exception);

        /// <summary>
        /// Displays an informational alert with a single button.
        /// </summary>
        /// <param name="title">
        /// The alert title.
        /// </param>
        /// <param name="message">
        /// A more descriptive explanation of the alert.
        /// </param>
        /// <param name="cancel">
        /// The text to display on the button; default is "Cancel".
        /// </param>
        Task ShowAlertAsync(string title, string message, string cancel = "Cancel");

        /// <summary>
        /// Displays an interrogatory alert with two buttons.
        /// </summary>
        /// <param name="title">
        /// The alert title.
        /// </param>
        /// <param name="message">
        /// A more descriptive explanation of the alert. Should describe the
        /// difference between choosing one button or the other.
        /// </param>
        /// <param name="accept">
        /// The text to display on the "accept" button.
        /// </param>
        /// <param name="cancel">
        /// The text to display on the "cancel" button.
        /// </param>
        /// <returns>
        /// A Task representing the user's selection. The task result contains
        /// true if the ""accept" button is selected, false if the "cancel"
        /// button is selected.
        /// </returns>
        Task<bool> ShowPromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel");

        /// <summary>
        /// Causes a repaint of the page. Should update any UI elements that may
        /// be stale.
        /// </summary>
        void RefreshView();

        /// <summary>
        /// Set the page state to "busy". Should display the particular
        /// platform's "waiting" indicator or animation.
        /// </summary>
        /// <param name="busy">
        /// Whether to mark the page is or not.
        /// </param>
        void SetIsBusy(bool busy = true);
    }	
}

