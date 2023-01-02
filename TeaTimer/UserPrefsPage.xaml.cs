using System;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <inheritdoc cref="Microsoft.Maui.Controls.ContentPage" />
public partial class UserPrefsPage : ContentPage
{
    #region Constructors
    public UserPrefsPage()
	{
		InitializeComponent();
        //AddCommand = new AddTeaCommand();
        //AddTeaButton.Command = AddCommand;
	}
    #endregion Constructors
}

internal class AddTeaCommand : ICommand
{
    public event EventHandler CanExecuteChanged
    {
        add { }
        remove { }
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    async public void Execute(object parameter)
    {
        await AppShell.Current.GoToAsync($"EditPage?TeaId={string.Empty}", true);
    }
}
