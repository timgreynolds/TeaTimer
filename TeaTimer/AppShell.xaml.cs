using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.mahonkin.tim.maui.TeaTimer.Pages;
using com.mahonkin.tim.maui.TeaTimer.Services;
using com.mahonkin.tim.maui.TeaTimer.Utilities;
using com.mahonkin.tim.maui.TeaTimer.ViewModels;
using com.mahonkin.tim.TeaDataService.DataModel;
using com.mahonkin.tim.TeaDataService.Services;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <inheritdoc cref="Shell"/>
public partial class AppShell : Shell
{
    /// <inheritdoc cref="Shell()" />
    public AppShell(INavigationService navigationService, IDataService<TeaModel> sqlService, ISettingsService settingsService)
    {
        Routing.RegisterRoute(nameof(EditPage), typeof(EditPage));
        InitializeComponent();

        if (FileSystemUtils.AppDataFileExists("kettle.mp3") == false) 
        {
            FileSystemUtils.CopyBundleAppDataResource("kettle.mp3");
        }

        try
        {
            InitDatabase(sqlService);
        }
        catch (Exception ex) // Last opportunity to capture bubbled-up exceptions.
        {
            Console.WriteLine("An exception occurred. {Type} - {Message}", ex.GetType().Name, ex.Message);
            if (BindingContext.GetType().IsAssignableTo(typeof(BaseViewModel)))
            {
                BaseViewModel baseViewModel = BindingContext as BaseViewModel;
                baseViewModel.DisplayService.ShowExceptionAsync(ex);
            }
        }
    }

    private static void InitDatabase(IDataService<TeaModel> sqlService)
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
                Console.WriteLine("No database file found. Unpacking from the app bundle.");
                FileSystemUtils.CopyBundleAppDataResource(dbFile);
                sqlService.Initialize(FileSystemUtils.GetAppDataFileFullName(dbFile));
            }

            List<TeaModel> teas = sqlService.Get();
            if (teas.Count < 1)
            {
                Console.WriteLine("No teas found in the tea database.");
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("An exception occurred. {Type} - {Message}", ex.GetType().Name, ex.Message);
        }
    }
}