using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		public BaseViewModel()
		{
		}

        public event PropertyChangedEventHandler PropertyChanged;

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

