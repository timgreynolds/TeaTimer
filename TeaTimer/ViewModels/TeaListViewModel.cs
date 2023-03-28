using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using com.mahonkin.tim.maui.TeaTimer.DataModel;
using com.mahonkin.tim.maui.TeaTimer.Services;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    public class TeaListViewModel : BaseViewModel
    {
        #region Private Fields
        private IList _teas;
        private TeaModel _selectedTea;
        #endregion Private Fields

        #region Public Properties
        public bool UseCelsius { get; }

        public IList Teas
        {
            get => _teas;
            private set => SetProperty(ref _teas, value);
        }

        public TeaModel SelectedTea
        {
            get => _selectedTea;
            set
            {
                SetProperty(ref _selectedTea, value);
                if (value is not null) 
                {
                    OnSelectedTeaChanged();
                }
            }
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
        #endregion Public Properties

        #region Constructors
        public TeaListViewModel(TeaNavigationService navigationService, TeaDisplayService displayService, TeaSqlService sqlService)
            : base(navigationService, displayService, sqlService)
        {
            RefreshList = new Command(async () => await  RefreshTeas());
            AddTeaCommand = new Command(() => AddTea());
            RefreshTeas();
        }
        #endregion Constructors

        #region Private Methods
        private async Task RefreshTeas()
        {
            Teas = await SqlService.GetAsync();
        }

        private void AddTea()
        {
            System.Console.WriteLine("Add Tea");
            NavigationService.NavigateToAsync(nameof(Pages.EditPage));
        }

        private void OnSelectedTeaChanged()
        {
            NavigationService.NavigateToAsync(nameof(Pages.EditPage), new Dictionary<string, object>() { { "Tea", _selectedTea } });
        }
        #endregion Private Methods
    }
}