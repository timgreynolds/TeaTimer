using System;
using System.Reflection;
using System.Windows.Input;
using AppKit;
using com.mahonkin.tim.maui.TeaTimer.ViewModels;
using Log = CoreFoundation.OSLog;
using LogLevel = CoreFoundation.OSLogLevel;
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
    private static readonly Log _logger = new Log(Assembly.GetExecutingAssembly().GetName().Name, nameof(AppDelegate));

    /// <inheritdoc cref="MauiProgram.CreateMauiApp()"/>
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    /// <inheritdoc cref="MauiUIApplicationDelegate.BuildMenu(IUIMenuBuilder)" />
    public override void BuildMenu(IUIMenuBuilder builder)
    {
        _logger.Log(LogLevel.Debug, "Building MacOS application menu.");
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
        _logger.Log(LogLevel.Debug, "Menu built; app starting.");
    }

    private void DeleteTea()
    {
        _logger.Log(LogLevel.Debug, "Delete Tea menu item selected.");
        if (CanExecute(AppShell.Current.CurrentPage, "DeleteTeaCommand", out ICommand deleteCommand))
        {
            deleteCommand.Execute(null);
            _logger.Log(LogLevel.Debug, "Delete command request executed.");
        }
        else
        {
            NotSupported();
            _logger.Log(LogLevel.Debug, "Delete is not currently supported.");
        }
    }

    private void EditTea()
    {
        _logger.Log(LogLevel.Debug, "Edit Tea menu item selected.");
        if (CanExecute(AppShell.Current.CurrentPage, "EditTeaCommand", out ICommand editCommand))
        {
            editCommand.Execute(null);
            _logger.Log(LogLevel.Debug, "Edit command request executed.");
        }
        else
        {
            NotSupported();
            _logger.Log(LogLevel.Debug, "Delete is not currently supported.");
        }
    }

    private void NotSupported()
    {
        AppShell.Current.CurrentPage.DisplayAlert("Not Supported", "The requested action is not supported in the current context.", "OK");
        _logger.Log(LogLevel.Debug, "Action not supported alert displayed.");
    }

    private bool CanExecute(Page page, string commandName, out ICommand command)
    {
        _logger.Log(LogLevel.Debug, $"Checking whether {commandName} can be executed from {page}");
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
        _logger.Log(LogLevel.Debug, $"{commandName} can be executing from {page} is {canExecute}");
        return canExecute;
    }
}

