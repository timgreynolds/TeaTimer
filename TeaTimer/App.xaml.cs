using com.mahonkin.tim.maui.TeaTimer.Services;
using com.mahonkin.tim.TeaDataService.DataModel;
using com.mahonkin.tim.TeaDataService.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer
{
    /// <inheritdoc cref="Application" />
    public partial class App : Application
    {
        /// <inheritdoc cref="Application.Application()" />
        public App(INavigationService navigationService, IDataService<TeaModel> sqlService, ISettingsService settingsService, ILoggerFactory loggerFactory)
        {
            InitializeComponent();

            MainPage = new AppShell(navigationService, sqlService, settingsService, loggerFactory);
            UserAppTheme = AppTheme.Light;
            RequestedThemeChanged += (sender, args) => UserAppTheme = args.RequestedTheme;
        }
    }
}