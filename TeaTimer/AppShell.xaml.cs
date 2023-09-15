using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using com.mahonkin.tim.maui.TeaTimer.Pages;
using com.mahonkin.tim.maui.TeaTimer.Services;
using com.mahonkin.tim.maui.TeaTimer.Utilities;
using com.mahonkin.tim.TeaDataService.DataModel;
using com.mahonkin.tim.TeaDataService.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls;

using Microsoft.Maui.Storage;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <inheritdoc cref="Shell"/>
public partial class AppShell : Shell
{
    private static ILogger _logger;

    public static ILogger Logger => _logger;

    /// <inheritdoc cref="TeaNavigationService" />
    public AppShell(INavigationService navigationService, IDataService<TeaModel> sqlService, ISettingsService settingsService, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

        Routing.RegisterRoute(nameof(EditPage), typeof(EditPage));

        try
        {
            InitializeComponent();

            string target = Path.Combine(FileSystem.AppDataDirectory, "Application Support", Assembly.GetExecutingAssembly().GetName().Name, "TeaVarieties.db3");
            if (File.Exists(target))
            {
                Logger.LogDebug($"Found database file at {target}; creating connection.");
                sqlService.Initialize(target);
            }
            else
            {
                Logger.LogWarning($"No database file found. Unpacking from the app bundle.");
                FileSystemUtils.CopyBundleAppDataResource(target).Wait();
                Logger.LogDebug("Tea database copied from app bundle to device; initializing.");
                sqlService.Initialize(target);
            }
            Logger.LogDebug($"Checking database contents.");
            List<TeaModel> teas = sqlService.Get();
            if (teas.Count < 1)
            {
                Logger.LogError("No teas found in the tea database.");
            }
            else
            {
                Logger.LogDebug($"Tea database contains {teas.Count} teas.");
            }
        }
        catch (System.Exception ex) // Maybe last top-level opportunity to display an exception alert
        {
            Logger.LogCritical($"An exception occurred. {ex.GetType().Name} - {ex.Message}");
        } 
    }
}