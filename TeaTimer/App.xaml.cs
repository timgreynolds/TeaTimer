using com.mahonkin.tim.maui.TeaTimer.Services;
using com.mahonkin.tim.TeaDataService.DataModel;
using com.mahonkin.tim.TeaDataService.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer
{
    /// <inheritdoc cref="Application" />
    public partial class TeaTimerApp : Application
    {
        private INavigationService _navigationService;
        private IDataService<TeaModel> _sqlService;
        private ISettingsService _settingsService;
        private ILoggerFactory _loggerFactory;
        private ILogger<TeaTimerApp> _logger;

        /// <inheritdoc cref="Application.Application()" />
        public TeaTimerApp(INavigationService navigationService, IDataService<TeaModel> sqlService, ISettingsService settingsService, ILoggerFactory loggerFactory)
        {
            _navigationService = navigationService;
            _sqlService = sqlService;
            _settingsService = settingsService;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<TeaTimerApp>();
            _logger.LogTrace("Constructor entered.");

            InitializeComponent();

            UserAppTheme = Microsoft.Maui.ApplicationModel.AppTheme.Light;
            RequestedThemeChanged += (sender, args) => UserAppTheme = args.RequestedTheme;
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            _logger.LogTrace("Create Window.");
            return new Window(new AppShell(_navigationService, _sqlService, _settingsService, _loggerFactory));
        }
    }
}