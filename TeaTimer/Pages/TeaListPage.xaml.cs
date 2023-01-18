using com.mahonkin.tim.maui.TeaTimer.ViewModels;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer.Pages;

public partial class TeaListPage : ContentPage
{
    public TeaListPage(TeaListViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}
