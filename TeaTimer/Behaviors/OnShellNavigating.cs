using com.mahonkin.tim.maui.TeaTimer.ViewModels;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer.Behaviors
{
    /// <inheritdoc cref="Behavior{T}" />
    public class OnShellNavigating : Behavior<AppShell>
    {
        protected override void OnAttachedTo(AppShell shell)
        {
            shell.Navigated += AddShellNavigated;
            base.OnAttachedTo(shell);
        }

        protected override void OnDetachingFrom(AppShell shell)
        {
            shell.Navigated -= AddShellNavigated;
            base.OnDetachingFrom(shell);
        }

        private void AddShellNavigated(object sender, ShellNavigatedEventArgs args)
        {
            object bc = ((AppShell)sender).CurrentPage.BindingContext;
            ((BaseViewModel)bc).ShellNavigated(sender, args);
        }
    }
}

