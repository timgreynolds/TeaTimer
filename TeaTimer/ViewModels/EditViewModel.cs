using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using com.mahonkin.tim.maui.TeaTimer.Services;
using com.mahonkin.tim.TeaDataService.DataModel;
using com.mahonkin.tim.TeaDataService.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer.ViewModels
{
    /// <summary>
    /// Viewmodel that backs up the <see cref="Pages.EditPage">Edit Tea</see>
    /// page.
    /// </summary>
    public class EditViewModel : BaseViewModel, IQueryAttributable
    {
        #region Private Fields
        private string _name;
        private string _backButtonLabel = "Back";
        private int _brewTemp = 212;
        private bool _isPageDirty;
        private TimeSpan _steepTime = TimeSpan.FromMinutes(2);
        private TeaModel _tea = new TeaModel(string.Empty);
        private ILogger<EditViewModel> _logger;
        #endregion Private Fields

        #region Public Properties
        /// <summary>
        /// The Name of the tea.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    IsPageDirty = true;
                    _tea.Name = value;
                }
                SetProperty(ref _name, value);
            }
        }

        /// <summary>
        /// Label to display on the Back Button.
        /// </summary>
        [Obsolete()]
        public string BackButtonLabel
        {
            get => _backButtonLabel;
            private set { }
        }

        /// <summary>
        /// The temperature at which the tea should steep.
        /// </summary>
        public int BrewTemp
        {
            get => _brewTemp;
            set
            {
                if (value != _brewTemp)
                {
                    IsPageDirty = true;
                    _tea.BrewTemp = value;
                }
                SetProperty(ref _brewTemp, value);
            }
        }

        /// <summary>
        /// Whether to display the <see cref="BrewTemp"/> in Celsius or Farenheit degress.
        /// </summary>
        public bool UseCelsius { get; }

        /// <summary>
        /// Whether the page should be considered 'dirty' and have the 'Save' button enabled.
        /// </summary>
        public bool IsPageDirty
        {
            get => _isPageDirty;
            private set => SetProperty(ref _isPageDirty, value);
        }

        /// <summary>
        /// The amount of time for which the steep.
        /// </summary>
        public TimeSpan SteepTime
        {
            get => _steepTime;
            set
            {
                if (value != _steepTime)
                {
                    IsPageDirty = true;
                    _tea.SteepTime = value;
                }
                SetProperty(ref _steepTime, value);
            }
        }

        /// <summary>
        /// Set the command to run when the Back button is selected.
        /// </summary>
        public Command BackButtonCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Set the command to run when the 'Save' button is selected.
        /// </summary>
        public Command SaveBtnPressed
        {
            get;
            private set;
        }
        #endregion Public Properties

        /// <summary>
        /// Viewmodel that backs up the Edit Tea page.
        /// </summary>
        /// <param name="navigationService"><see cref="TeaNavigationService"/></param>
        /// <param name="displayService"><see cref="TeaDisplayService"/></param>
        /// <param name="sqlService"><see cref="TeaSqlService{TeaModel}"/></param>
        public EditViewModel(INavigationService navigationService, IDisplayService displayService, IDataService<TeaModel> sqlService, ISettingsService settingsService, ILoggerFactory loggerFactory)
           : base(navigationService, displayService, sqlService, settingsService)
        {
            _logger = loggerFactory.CreateLogger<EditViewModel>();
            _logger.LogTrace("Constructor entered.");
            SaveBtnPressed = new Command(async () => await Save());
            BackButtonCommand = new Command(async () => await NavigateBack());
        }

        /// <inheritdoc cref="ApplyQueryAttributes(IDictionary{string, object})" />
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Tea", out object param))
            {
                if (param.GetType().IsAssignableTo(typeof(TeaModel)))
                {
                    _tea = param as TeaModel;
                }
                else
                {
                    _tea = new TeaModel();
                }
            }
            else
            {
                _tea = new TeaModel();
            }
            Name = _tea.Name;
            BrewTemp = _tea.BrewTemp;
            SteepTime = _tea.SteepTime;
            IsPageDirty = false;
        }

        private async Task Save()
        {
            if (_tea.Id > 0)
            {
                try
                {
                    _tea = await SqlService.UpdateAsync(_tea);
                    await NavigateBack();
                }
                catch (Exception ex)
                {
                    await DisplayService.ShowExceptionAsync(ex);
                }
            }
            else
            {
                try
                {
                    _tea = await SqlService.AddAsync(_tea);
                    await NavigateBack();
                }
                catch (Exception ex)
                {
                    await DisplayService.ShowExceptionAsync(ex);
                }
            }
        }

        private async Task  NavigateBack()
        {
            await NavigationService.GoBackAsync(true);
        }
    }
}