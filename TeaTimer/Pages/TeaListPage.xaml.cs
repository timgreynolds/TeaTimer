using com.mahonkin.tim.maui.TeaTimer.ViewModels;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer.Pages;

/// <inheritdoc cref="ContentPage"/>
public partial class TeaListPage : ContentPage
{
    /// <inheritdoc cref="ContentPage()" />
    public TeaListPage(TeaListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}