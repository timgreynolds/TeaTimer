using com.mahonkin.tim.maui.TeaTimer.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;

namespace com.mahonkin.tim.maui.TeaTimer.Pages;

public partial class TimerPage : ContentPage
{
	public TimerPage(TimerViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
