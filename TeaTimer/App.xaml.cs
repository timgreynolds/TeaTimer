using com.mahonkin.tim.maui.TeaTimer.Services;
using com.mahonkin.tim.TeaDataService.DataModel;
using com.mahonkin.tim.TeaDataService.Services;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer
{
    /// <inheritdoc cref="Application" />
    public partial class TeaTimerApp : Application
    {
        /// <inheritdoc cref="Application.Application()" />
        public TeaTimerApp(INavigationService navigationService, IDataService<TeaModel> sqlService, ISettingsService settingsService)
        {
            InitializeComponent();
            
            MainPage = new AppShell(navigationService, sqlService, settingsService);
            UserAppTheme = TeaTimerApp.Current.RequestedTheme;
            RequestedThemeChanged += (sender, args) => UserAppTheme = args.RequestedTheme;
        }
    }
}