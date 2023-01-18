using System;
using System.Collections.Generic;
using com.mahonkin.tim.maui.TeaTimer.Services;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
	public class EditViewModel : BaseViewModel, IQueryAttributable
	{
		public EditViewModel(INavigationService navigationService) : base(navigationService)
		{
		}

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Console.WriteLine("Apply Query Atrributes");
        }
    }
}

