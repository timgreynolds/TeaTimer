using Microsoft.Maui.Controls;
using com.mahonkin.tim.maui.TeaTimer.DataModel;
using System;
using Timer = System.Timers.Timer;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Accessibility;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Media;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <inheritdoc cref="Microsoft.Maui.Controls.ContentPage" />
public partial class MainPage : ContentPage
{
    #region Private Fields
    private static readonly Timer _timer = new Timer(1000);
    private static TimeSpan _timeLeft = new TimeSpan(0, 0, 0);
    #endregion Private Fields

    #region Constructors
    /// <inheritdoc cref="Microsoft.Maui.Controls.ContentPage.ContentPage()" />
    public MainPage()
    {
        InitializeComponent();
        try
        {
            MainLabel.Text = _timeLeft.ToString(@"mm\:ss");
            StartStopBtn.IsEnabled = false;
            TeaPicker.ItemsSource = TeaModel.Teas;
            _timer.AutoReset = true;
            _timer.Elapsed += (sender, e) => MainThread.BeginInvokeOnMainThread(() => UpdateLabel(sender));
        }
        catch (Exception ex)
        {
            DisplayAlert($"An Exception was thrown.\n{ex.Message}", ex.StackTrace, "OK");
        }
    }
    #endregion Constructors

    #region Public Methods
    /// <summary>
    /// Method fired when the selection on the TeaPicker changes.
    /// </summary>
    /// <param name="sender">The object sending the event. In this case the Tea Picker.</param>
    /// <param name="e">Event Data.</param>
    public void TeaPicker_SelectedIndexChanged(Object sender, EventArgs e)
    {
        //Picker TeaPicker = sender as Picker;
        if (TeaPicker.SelectedIndex >= 0)
        {
            string degSym = "\u2109";
            TeaModel selectedTea = TeaPicker.SelectedItem as TeaModel;
            if (App.UseCelsius)
            {
                selectedTea.BrewTemp = (int)UnitConverters.FahrenheitToCelsius(selectedTea.BrewTemp); //(((selectedTea.BrewTemp - 32) * 5) / 9);
                degSym = "\u2103";
            }
            Title = $"{selectedTea.Name}, Brew Temp {selectedTea.BrewTemp}{degSym}, Steep Time {selectedTea.SteepTime.ToString(@"mm\:ss")}";
            _timeLeft = selectedTea.SteepTime;
            MainLabel.Text = _timeLeft.ToString(@"mm\:ss");
            StartStopBtn.IsEnabled = true;
        }
        else if (StartStopBtn.IsEnabled)
        {
            StartStopBtn.IsEnabled = false;
            _timeLeft = new TimeSpan(0, 0, 0);
            MainLabel.Text = _timeLeft.ToString(@"mm\:ss");
            Title = "Welcome to Tea Timer!";
        }
    }

    /// <summary>
    /// Method fired when the Start/Stop Button is pressed.
    /// </summary>
    /// <param name="sender">The object sending the event. In this case the Start/Stop Button.</param>
    /// <param name="e">Event Data</param>
    public void StartStopBtn_Clicked(Object sender, EventArgs e)
    {
        if (_timer.Enabled)
        {
            _timer.Stop();
            StartStopBtn.Text = $"Start";
            TeaPicker.IsEnabled = true;
        }
        else
        {
            _timer.Start();
            StartStopBtn.Text = $"Stop";
            TeaPicker.IsEnabled = false;
        }

        SemanticScreenReader.Announce(StartStopBtn.Text);
    }
    #endregion Public Methods

    #region Private Methods
    private void UpdateLabel(object sender)
    {
        if (_timeLeft.TotalSeconds >= 0)
        {
            MainLabel.Text = _timeLeft.ToString(@"mm\:ss");
            _timeLeft = _timeLeft.Subtract(new TimeSpan(0, 0, 1));
        }
        else
        {
            _timer.Stop();
            StartStopBtn.Text = "Start";
            StartStopBtn.IsEnabled = false;
            TeaPicker.IsEnabled = true;
            MainLabel.Text = "Your tea is ready!";
        }

        SemanticScreenReader.Announce(MainLabel.Text);
    }
    #endregion Private Methods
}
