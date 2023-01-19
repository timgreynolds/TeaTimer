using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using com.mahonkin.tim.maui.TeaTimer.Services;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
		public INavigationService NavigationService { get; private set; }
		public IDisplayService DisplayService { get; private set; }

        public BaseViewModel(INavigationService navigationService, IDisplayService displayService)
		{
			NavigationService = navigationService;
			DisplayService = displayService;
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

