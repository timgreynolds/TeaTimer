using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using com.mahonkin.tim.maui.TeaTimer.Services;
using com.mahonkin.tim.TeaDataService.DataModel;
using com.mahonkin.tim.TeaDataService.Services;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    /// <summary>
    /// Viewmodel that backs up the <see cref="Pages.TeaListPage">Tea List
    /// </see> page.
    /// </summary>
    public partial class TeaListViewModel : BaseViewModel
    {
        #region Private Fields
        private bool _isBusy = false;
        private bool _isSelected = false;
        private IList _teas;
        private TeaModel _selectedTea;
        #endregion Private Fields

        #region Public Properties
        /// <summary>
        /// Whether to display the Brew Temperature in Celsius or Farenheit
        /// degrees.
        /// </summary>
        public bool UseCelsius { get; }

        /// <summary>
        /// Whether the page should be considered 'busy' the waiting symbol or
        /// animation should be displayed.
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            private set => SetProperty(ref _isBusy, value);
        }

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
        /// Refreshes the list of teas from the data provider and resets the
        /// contents of the list.
        /// </summary>
        public ICommand RefreshList
        {
            get;
            private set;
        }

        /// <summary>
        /// Displays the Add Tea page.
        /// </summary>
        public ICommand AddTeaCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Displays the Edit Tea page with the information from
        /// <see cref="SelectedTea"/>
        /// </summary>
        public ICommand EditTeaCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Deletes the <see cref="SelectedTea" />
        /// </summary>
        public ICommand DeleteTeaCommand
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
        public TeaListViewModel(INavigationService navigationService, IDisplayService displayService, IDataService<TeaModel> sqlService)
            : base(navigationService, displayService, sqlService)
        {
            RefreshList = new Command(async () => await RefreshTeas(this, EventArgs.Empty));
            AddTeaCommand = new Command(AddTea);
            EditTeaCommand = new Command(async (p) => await EditTea(p));
            DeleteTeaCommand = new Command(async (p) => await DeleteTea(p));
            navigationService.ShellNavigated += async (sender, args) => await RefreshTeas(sender, args);
        }
        #endregion Constructors

        #region Private Methods
        private async Task RefreshTeas(object sender, EventArgs args)
        {
            IsBusy = true;
            Teas = await SqlService.GetAsync();
            _selectedTea = Teas[0] as TeaModel;
            IsBusy = false;
        }

        private void AddTea()
        {
            NavigationService.NavigateToAsync(nameof(Pages.EditPage), null);
        }

        private async Task DeleteTea(object parameters)
        {
            bool delete = false;
            if (_selectedTea != null)
            {
                delete = await DisplayService.ShowPromptAsync("Delete?", $"Are you sure you want to delete {_selectedTea.Name} from the database?", "Delete", "Cancel");
            }
            else if (parameters != null && parameters.GetType().IsAssignableTo(typeof(TeaModel)))
            {
                delete = await DisplayService.ShowPromptAsync("Delete?", $"Are you sure you want to delete {((TeaModel)parameters).Name} from the database?", "Delete", "Cancel");
            }
            else
            {
                await DisplayService.ShowAlertAsync("Warning!", "No tea selected. Please select a tea and try again.", "OK");
            }
            if (delete)
            {
                try
                {
                    bool success = await SqlService.DeleteAsync(_selectedTea ?? (TeaModel)parameters);
                    if (success)
                    {
                        await DisplayService.ShowAlertAsync("Deleted", "Tea was successfully deleted", "OK");
                    }
                    else
                    {
                        await DisplayService.ShowAlertAsync("Error", "An error occurred.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayService.ShowExceptionAsync(ex);
                }
                finally
                {
                    RefreshList.Execute(null);
                    DisplayService.RefreshView();
                }
            }
        }

        private async Task EditTea(object parameters)
        {
            if (_selectedTea != null)
            {
                await NavigationService.NavigateToAsync(nameof(Pages.EditPage), new Dictionary<string, object>() { { "Tea", _selectedTea } });
            }
            else if (parameters != null && parameters.GetType().IsAssignableTo(typeof(TeaModel)))
            {
                await NavigationService.NavigateToAsync(nameof(Pages.EditPage), new Dictionary<string, object>() { { "Tea", (TeaModel)parameters } });
            }
            else
            {
                await DisplayService.ShowAlertAsync("Warning!", "No tea selected. Please select a tea and try again.", "OK");
            }
        }
        #endregion Private Methods
    }
}