using com.mahonkin.tim.maui.TeaTimer.Services;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public bool UseCelsius
        {
            get => TeaSettingsService.Get<bool>(nameof(UseCelsius), false);
            set
            {
                TeaSettingsService.Set<bool>(nameof(UseCelsius), value);
            }
        }

        public SettingsViewModel(TeaNavigationService navigationService, TeaDisplayService displayService, TeaSettingsService settingsService)
            : base(navigationService, displayService, settingsService) { }
    }
}

