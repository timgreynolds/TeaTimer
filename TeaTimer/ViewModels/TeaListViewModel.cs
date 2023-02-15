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
        private bool _useCelsius;
        #endregion Private Fields

        #region Public Properties
        public IList Teas
        {
            get => _teas;
            private set { }
        }

        public TeaModel SelectedTea
        {
            get => _selectedTea;
            set
            {
                SetProperty(ref _selectedTea, value);
                if (value is not null) //&& _selectedTea != value)
                {
                    OnSelectedTeaChanged();
                }
            }
        }

        public bool UseCelsius
        {
            get => _useCelsius;
            private set { }
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
        public TeaListViewModel(TeaNavigationService navigationService, TeaDisplayService displayService, TeaSettingsService settingsService)
            : base(navigationService, displayService, settingsService)
        {
            _useCelsius = settingsService.Get(nameof(UseCelsius), false);
            RefreshList = new Command(() => RefreshTeas());
            AddTeaCommand = new Command(() => AddTea());
            RefreshTeas();
        }
        #endregion Constructors

        #region Private Methods
        private void RefreshTeas()
        {
            _teas = TeaModel.Teas;
        }

        private void AddTea()
        {
            System.Console.WriteLine("Add Tea");
            TeaNavigationService.NavigateToAsync(nameof(Pages.EditPage));
        }

        private void OnSelectedTeaChanged()
        {
            TeaNavigationService.NavigateToAsync(nameof(Pages.EditPage), new Dictionary<string, object>() { { "Tea", _selectedTea } });
        }
        #endregion Private Methods
    }
}