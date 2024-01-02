using System;
using Microsoft.Maui.ApplicationModel;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public partial class TeaSettingsService : ISettingsService
	{
        public bool UseCelsius { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public AppTheme AppTheme { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public partial void LoadDefaultSettings();
        public partial void SettingsChanged();
    }
}

