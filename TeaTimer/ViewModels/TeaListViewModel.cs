using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.mahonkin.tim.maui.TeaTimer.Services;
using com.mahonkin.tim.TeaDataService.Exceptions;
using com.mahonkin.tim.TeaDataService.DataModel;
using com.mahonkin.tim.TeaDataService.Services;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.Logging;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    /// <summary>
    /// Viewmodel that backs up the <see cref="Pages.TeaListPage">Tea List
    /// </see> page.
    /// </summary>
    public partial class TeaListViewModel : BaseViewModel
    {
        #region Private Fields
        private bool _isSelected = false;
        private bool _useCelsius = false;
        private IList _teas;
        private TeaModel _selectedTea;
        private ILogger<TeaListViewModel> _logger;
        #endregion Private Fields

        #region Public Properties
        /// <summary>
        /// Whether to display the Brew Temperature in Celsius or Farenheit
        /// degrees.
        /// </summary>
        public bool UseCelsius { get => _useCelsius; }

        /// <summary>
        /// Whether a tea variety has been selected.
        /// </summary>
        public bool IsTeaSelected
        {
            get => _isSelected;
            private set => SetProperty(ref _isSelected, value);
        }

        /// <summary>
        /// The list of teas retrieved from the data provider. 
        /// </summary>
        public IList Teas
        {
            get => _teas;
            set => SetProperty(ref _teas, value);
        }

        /// <summary>
        /// The tea that has been selected.
        /// </summary>
        public TeaModel SelectedTea
        {
            get => _selectedTea;
            set => SetProperty(ref _selectedTea, value);
        }

        /// <summary>
        /// Displays the Add Tea page.
        /// </summary>
        public Command AddTeaCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Displays the Edit Tea page with the information from
        /// <see cref="SelectedTea"/>
        /// </summary>
        public Command<TeaModel> EditTeaCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Deletes the <see cref="SelectedTea" />
        /// </summary>
        public Command<TeaModel> DeleteTeaCommand
        {
            get;
            private set;
        }
        #endregion Public Properties

        #region Constructors
        /// <summary>
        /// Viewmodel that backs up the Tea List page.
        /// </summary>
        /// <param name="navigationService"><see cref="TeaNavigationService"/></param>
        /// <param name="displayService"><see cref="TeaDisplayService"/></param>
        /// <param name="sqlService"><see cref="TeaSqlService{TeaModel}"/></param>
        public TeaListViewModel(INavigationService navigationService, IDisplayService displayService, IDataService<TeaModel> sqlService, ISettingsService settingsService, ILoggerFactory loggerFactory)
            : base(navigationService, displayService, sqlService, settingsService)
        {
            _logger = loggerFactory.CreateLogger<TeaListViewModel>();
            _logger.LogTrace("Constructor entered.");
            AddTeaCommand = new Command(async () => await AddTeaAsync());
            EditTeaCommand = new Command<TeaModel>(async (p) => await EditTea(p));
            DeleteTeaCommand = new Command<TeaModel>(async (p) => await DeleteTea(p));
            NavigationService.ShellNavigated += async (sender, args) => await RefreshTeas(sender, args);
        }
        #endregion Constructors

        #region Private Methods
        private async Task RefreshTeas(object sender, EventArgs args)
        {
            // IsBusy = true;
            try
            {
                Teas = await SqlService.GetAsync();
            }
            catch (TeaSqlException ex)
            {
                await DisplayService.ShowAlertAsync(ex.GetType().Name, $"A database error occurred.\n{ex.Result} - {ex.Message}");
            }
            catch (Exception ex)
            {
                await DisplayService.ShowExceptionAsync(ex);
            }
        }

        private async Task AddTeaAsync()
        {
            try
            {
                await NavigationService.NavigateToAsync(nameof(Pages.EditPage), null);
            }
            catch (Exception ex)
            {
                await DisplayService.ShowExceptionAsync(ex);
            }
        }

        private async Task DeleteTea(TeaModel parameter)
        {
            _logger.LogDebug("Delete Tea selected.");
            bool delete = false;
            TeaModel deleteTea = null;
            if (parameter != null)
            {
                _logger.LogDebug("Tea passed as Command Parameter " + parameter.Name);
                deleteTea = parameter;
                delete = await DisplayService.ShowPromptAsync("Delete?", $"Are you sure you want to delete {deleteTea.Name} from the database?", "Delete", "Cancel");
            }
            else if (SelectedTea != null)
            {
                _logger.LogDebug("No Command Parameter passed. Trying selected tea " + SelectedTea.Name);
                deleteTea = SelectedTea;
                delete = await DisplayService.ShowPromptAsync("Delete?", $"Are you sure you want to delete {deleteTea.Name} from the database?", "Delete", "Cancel");
            }
            else
            {
                _logger.LogWarning("No tea selected. Please select a tea and try again.");
                await DisplayService.ShowAlertAsync("Warning!", "No tea selected. Please select a tea and try again.", "OK");
            }
            if (delete && deleteTea is not null)
            {
                _logger.LogDebug("Deleting tea " + deleteTea.Name);
                try
                {
                    var success = await SqlService.DeleteAsync(deleteTea);
                    if ((bool)success)
                    {
                        _logger.LogDebug(deleteTea + " deleted.");
                        await DisplayService.ShowAlertAsync("Deleted", "Tea was successfully deleted", "OK");
                    }
                    else
                    {
                        _logger.LogDebug(deleteTea + " could not be deleted for unknown reasons.");
                        await DisplayService.ShowAlertAsync("Error", "An error occurred.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("An exception occurred " + ex.Message + "\n" + ex.StackTrace);
                    await DisplayService.ShowExceptionAsync(ex);
                }
                finally
                {
                    await RefreshTeas(this, EventArgs.Empty);
                    DisplayService.RefreshView();
                }
            }
        }

        private async Task EditTea(TeaModel parameter)
        {
            _logger.LogDebug("Edit Tea selected");
            if (parameter != null)
            {
                _logger.LogDebug("Tea passed as Command Parameter " + parameter.Name);
                await NavigationService.NavigateToAsync(nameof(Pages.EditPage), new Dictionary<string, object>() { { "Tea", parameter } });
            }
            else if (SelectedTea != null)
            {
                _logger.LogDebug("No Command Parameter passed. Trying selected tea " + SelectedTea.Name);
                await NavigationService.NavigateToAsync(nameof(Pages.EditPage), new Dictionary<string, object>() { { "Tea", SelectedTea } });
            }
            else
            {
                _logger.LogWarning("No tea selected. Please select a tea and try again.");
                await DisplayService.ShowAlertAsync("Warning!", "No tea selected. Please select a tea and try again.", "OK");
            }
        }
        #endregion Private Methods
    }
}