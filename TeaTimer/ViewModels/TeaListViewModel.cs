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
                if (value is not null && _selectedTea != value)
                {
                    OnSelectedTeaChanged(value);
                }
                SetProperty(ref _selectedTea, value);
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
            set;
        }
        #endregion Public Properties

        #region Constructors
        public TeaListViewModel(TeaNavigationService navigationService, TeaDisplayService displayService, TeaSettingsService settingsService)
            : base(navigationService, displayService, settingsService)
        {
            RefreshList = new Command(() => _teas = TeaModel.Teas);
            AddTeaCommand = new Command(() => AddTea());
            _teas = TeaModel.Teas;
        }
        #endregion Constructors

        #region Private Methods
        private void AddTea()
        {
            System.Console.WriteLine("Add Tea");
            TeaNavigationService.NavigateToAsync(nameof(Pages.EditPage));
        }

        private void OnSelectedTeaChanged(TeaModel tea)
        {
            TeaNavigationService.NavigateToAsync(nameof(Pages.EditPage), new Dictionary<string, object>() { { "Tea", tea } });
        }
        #endregion Private Methods
    }
}