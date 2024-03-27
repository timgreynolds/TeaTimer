using System;
using System.Collections.Generic;
using com.mahonkin.tim.maui.TeaTimer.Pages;
using com.mahonkin.tim.maui.TeaTimer.Services;
using com.mahonkin.tim.maui.TeaTimer.Utilities;
using com.mahonkin.tim.maui.TeaTimer.ViewModels;
using com.mahonkin.tim.TeaDataService.DataModel;
using com.mahonkin.tim.TeaDataService.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <inheritdoc cref="Shell"/>
public partial class AppShell : Shell
{
    private readonly ILogger _logger;

    /// <inheritdoc cref="Shell()" />
    public AppShell(INavigationService navigationService, IDataService<TeaModel> sqlService, ISettingsService settingsService, ILoggerFactory factory)
    {
        _logger = factory.CreateLogger<AppShell>();

        Routing.RegisterRoute(nameof(EditPage), typeof(EditPage));
        InitializeComponent();

        if (FileSystemUtils.AppDataFileExists("kettle.mp3") == false) 
        {
            _logger.LogDebug("Sound file not found, copying sound file from the app bundle.");
            FileSystemUtils.CopyBundleAppDataResource("kettle.mp3").Wait();
        }

        try
        {
            InitDatabase(sqlService);
        }
        catch (Exception ex) // Last opportunity to capture bubbled-up exceptions.
        {
            _logger.LogCritical("An exception occurred. {Type} - {Message}", ex.GetType().Name, ex.Message);
            if (BindingContext.GetType().IsAssignableTo(typeof(BaseViewModel)))
            {
                BaseViewModel baseViewModel = BindingContext as BaseViewModel;
                baseViewModel.DisplayService.ShowExceptionAsync(ex);
            }
        }
    }

    private void InitDatabase(IDataService<TeaModel> sqlService)
    {
        try
        {
            string dbFile = "TeaVarieties.db3";
            if (FileSystemUtils.AppDataFileExists(dbFile))
            {
                sqlService.Initialize(FileSystemUtils.GetAppDataFileFullName(dbFile));
            }
            else
            {
                _logger.LogDebug("No database file found. Unpacking from the app bundle.");
                FileSystemUtils.CopyBundleAppDataResource(dbFile).Wait();
                _logger.LogDebug("Database file copied from the app bundle to the app data directory.");
                _logger.LogDebug("Initializing database file {dbfile}", FileSystemUtils.GetAppDataFileFullName(dbFile));
                sqlService.Initialize(FileSystemUtils.GetAppDataFileFullName(dbFile));
            }

            List<TeaModel> teas = sqlService.Get();
            if (teas.Count < 1)
            {
                _logger.LogDebug("No teas found in the tea database.");
            }
        }
        catch (System.Exception ex)
        {
            _logger.LogCritical("An exception occurred. {Type} - {Message}", ex.GetType().Name, ex.Message);
        }
    }
}