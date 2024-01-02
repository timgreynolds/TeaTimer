using Microsoft.Maui.ApplicationModel;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public interface ISettingsService
    {
        bool UseCelsius { get; set; }
        AppTheme AppTheme { get; set; }
        void LoadDefaultSettings();
        void SettingsChanged();
    }
}