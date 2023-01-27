using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using com.mahonkin.tim.maui.TeaTimer.Services;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
		public TeaNavigationService TeaNavigationService { get; private set; }
		public TeaDisplayService TeaDisplayService { get; private set; }
		public TeaSettingsService TeaSettingsService { get; private set; }

        public BaseViewModel(TeaNavigationService navigationService, TeaDisplayService displayService, TeaSettingsService settingsService)
		{
			TeaNavigationService = navigationService;
			TeaDisplayService = displayService;
			TeaSettingsService = settingsService;
		}

		protected virtual void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if(EqualityComparer<T>.Default.Equals(storage, value) == false)
			{
				storage = value;
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
    }
}

