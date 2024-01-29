using Microsoft.Maui.ApplicationModel;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public partial interface ISettingsService
    {
        bool UseCelsius { get; set; }
        AppTheme AppTheme { get; set; }
        static partial void LoadDefaultSettings();
        static partial void SettingsChanged();
    }
}