using System;
using Microsoft.Maui.ApplicationModel;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public partial class TeaSettingsService : ISettingsService
	{
        public bool UseCelsius { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public AppTheme AppTheme { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public static partial void LoadDefaultSettings();
        public static partial void SettingsChanged();
    }
}

