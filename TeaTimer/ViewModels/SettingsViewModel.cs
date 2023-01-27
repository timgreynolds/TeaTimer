using com.mahonkin.tim.maui.TeaTimer.Services;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    public class SettingsViewModel : BaseViewModel
	{
		private bool _useCelsius;

		public bool UseCelsius
		{
			get => _useCelsius;
			set
			{
				if (_useCelsius != value)
				{
					TeaSettingsService.Set<bool>(nameof(UseCelsius), value);
                }
                SetProperty(ref _useCelsius, value);
            }
		}

        public SettingsViewModel(TeaNavigationService navigationService, TeaDisplayService displayService, TeaSettingsService settingsService)
			: base(navigationService, displayService, settingsService)
		{
			if (TeaSettingsService.ContainsKey(nameof(UseCelsius)))
            {
                _useCelsius = TeaSettingsService.Get<bool>(nameof(UseCelsius), false);
            }
		}
	}
}

