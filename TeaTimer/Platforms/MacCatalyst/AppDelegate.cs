using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AppKit;
using com.mahonkin.tim.maui.TeaTimer.Services;
using com.mahonkin.tim.maui.TeaTimer.ViewModels;
using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Hosting;
using UIKit;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <summary>
/// A set of methods to manage shared behaviors for your app.   
/// </summary>
[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    private NSObject _observer;

    /// <inheritdoc cref="MauiProgram.CreateMauiApp()"/>
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    /// <inheritdoc cref="MauiUIApplicationDelegate.BuildMenu(IUIMenuBuilder)" />
    public override void BuildMenu(IUIMenuBuilder builder)
    {
        builder.RemoveMenu(UIMenuIdentifier.Format.GetConstant());
        builder.RemoveMenu(UIMenuIdentifier.Services.GetConstant());
        builder.RemoveMenu(UIMenuIdentifier.View.GetConstant());
        builder.RemoveMenu(UIMenuIdentifier.Speech.GetConstant());
        builder.RemoveMenu(UIMenuIdentifier.Spelling.GetConstant());
        builder.RemoveMenu(UIMenuIdentifier.StandardEdit.GetConstant());
        builder.RemoveMenu(UIMenuIdentifier.Substitutions.GetConstant());
        builder.RemoveMenu(UIMenuIdentifier.Transformations.GetConstant());
        builder.RemoveMenu(UIMenuIdentifier.UndoRedo.GetConstant());

        UIAction editAction = UIAction.Create("Edit Tea", null, "EditTea", (action) => EditTea());
        UIAction deleteAction = UIAction.Create("Delete Tea", null, "DeleteTea", (action) => DeleteTea());
        UIMenuElement[] actions = new UIMenuElement[] { editAction, deleteAction };
        UIMenu teasMenu = UIMenu.Create(string.Empty, null, UIMenuIdentifier.None, UIMenuOptions.DisplayInline, actions);

        builder.InsertChildMenuAtStart(teasMenu, UIMenuIdentifier.Edit.GetConstant());

        base.BuildMenu(builder);
    }

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        LoadDefaultPrefs();
        _observer = NSNotificationCenter.DefaultCenter.AddObserver((NSString)"NSUserDefaultsDidChangeNotification", (n) => DefaultsChanged());
        DefaultsChanged();
        return base.FinishedLaunching(application, launchOptions);
    }

    public override void WillTerminate(UIApplication application)
    {
        if (_observer != null)
        {
            NSNotificationCenter.DefaultCenter.RemoveObserver(_observer);
            _observer = null;
        }
    }

    private void DeleteTea()
    {
        if (CanExecute(AppShell.Current.CurrentPage, "DeleteTeaCommand", out ICommand deleteCommand))
        {
            deleteCommand.Execute(null);
        }
        else
        {
            NotSupported();
        }
    }

    private void EditTea()
    {
        if (CanExecute(AppShell.Current.CurrentPage, "EditTeaCommand", out ICommand editCommand))
        {
            editCommand.Execute(null);
        }
        else
        {
            NotSupported();
        }
    }

    private void NotSupported()
    {
        AppShell.Current.CurrentPage.DisplayAlert("Not Supported", "The requested action is not supported in the current context.", "OK");
    }

    private bool CanExecute(Page page, string commandName, out ICommand command)
    {
        bool canExecute = false;
        command = null;
        if (page.BindingContext is TeaListViewModel)
        {
            TeaListViewModel viewModel = page.BindingContext as TeaListViewModel;

            if (nameof(viewModel.DeleteTeaCommand).Equals(commandName, StringComparison.OrdinalIgnoreCase))
            {
                command = viewModel.DeleteTeaCommand;
            }
            else if (nameof(viewModel.EditTeaCommand).Equals(commandName, StringComparison.OrdinalIgnoreCase))
            {
                command = viewModel.EditTeaCommand;
            }

            if (command is not null && viewModel.DeleteTeaCommand.CanExecute(null))
            {
                canExecute = true;
            }
        }
        return canExecute;
    }

    private void LoadDefaultPrefs()
    {
        TeaSettingsService.LoadDefaultSettings();
    }

    private void DefaultsChanged()
    {
        TeaSettingsService.SettingsChanged();
    }
}