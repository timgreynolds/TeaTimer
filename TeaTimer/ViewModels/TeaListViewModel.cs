using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using com.mahonkin.tim.maui.TeaTimer.DataModel;
using com.mahonkin.tim.maui.TeaTimer.Services;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    public partial class TeaListViewModel : BaseViewModel
    {
        #region Private Fields
        private bool _isBusy = false;
        private bool _isSelected = false;
        private IList _teas;
        private TeaModel _selectedTea;
        #endregion Private Fields

        #region Public Properties
        public bool UseCelsius { get; }

        public bool IsBusy
        {
            get => _isBusy;
            private set => SetProperty(ref _isBusy, value);
        }

        public bool IsTeaSelected
        {
            get
            {
                if (_selectedTea == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            private set => SetProperty(ref _isSelected, value);
        }

        public IList Teas
        {
            get => _teas;
            set => SetProperty(ref _teas, value);
        }

        public TeaModel SelectedTea
        {
            get => _selectedTea;
            set => SetProperty(ref _selectedTea, value);
        }

        public ICommand RefreshList
        {
            get;
            private set;
        }

        public ICommand AddTeaCommand
        {
            get;
            private set;
        }

        public ICommand EditTeaCommand
        {
            get;
            private set;
        }

        public ICommand DeleteTeaCommand
        {
            get;
            private set;
        }
        #endregion Public Properties

        #region Constructors
        public TeaListViewModel(TeaNavigationService navigationService, TeaDisplayService displayService, TeaSqlService sqlService)
            : base(navigationService, displayService, sqlService)
        {
            RefreshList = new Command(() => RefreshTeas(this, EventArgs.Empty));
            AddTeaCommand = new Command(AddTea);
            EditTeaCommand = new Command(async (p) => await EditTea(p), (p) => EditDeleteCanExecute(p));
            DeleteTeaCommand = new Command(async (p) => await DeleteTea(p), (p) => EditDeleteCanExecute(p));
            navigationService.ShellNavigated += (sender, args) => RefreshTeas(sender, args);
        }
        #endregion Constructors

        #region Private Methods
        private void RefreshTeas(object sender, EventArgs args)
        {
            DisplayService.SetIsBusy(true);
            Teas = SqlService.Get();
            DisplayService.RefreshView();
            DisplayService.SetIsBusy(false);
            IsBusy = false;
        }

        private void AddTea()
        {
            NavigationService.NavigateToAsync(nameof(Pages.EditPage));
        }

        private bool EditDeleteCanExecute(object parameters)
        {
                return IsTeaSelected;
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
                bool success = await SqlService.DeleteAsync(_selectedTea ?? (TeaModel)parameters);
                if(success)
                {
                    await DisplayService.ShowAlertAsync("Deleted", "Tea was successfully deleted", "OK");
                }
                else
                {
                    await DisplayService.ShowAlertAsync("Error", "An error occurred.", "OK");
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