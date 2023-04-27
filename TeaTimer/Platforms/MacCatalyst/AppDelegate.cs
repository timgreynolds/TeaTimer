using System;
using System.Windows.Input;
using AppKit;
using com.mahonkin.tim.maui.TeaTimer.ViewModels;
using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Hosting;
using UIKit;

namespace com.mahonkin.tim.maui.TeaTimer;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

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

    private void DeleteTea()
    {
        if (CanExecute(AppShell.Current.CurrentPage, "DeleteTeaCommand", out ICommand deleteCommand))
        {
            deleteCommand.Execute(null);
        }
        else NotSupported();
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
}

