using System;
using System.Timers;
using AppKit;
using Foundation;

namespace TeaTimer
{
    public partial class MainViewController : NSViewController
    {
        #region private fields
        private TeaModel _selectedTea;
        private TeaVarietiesDataSource _datasource;
        private Timer _timer = new Timer(1000.0);
        private TimeSpan _steepTime;
        private NSButton _startStopButton;
        #endregion

        public MainViewController(IntPtr handle) : base(handle)
        {
        }

        #region override methods
        public override void ViewWillAppear()
        {
            base.ViewWillAppear();

            try
            {
                // Setup the ComboBox datasource
                _datasource = new TeaVarietiesDataSource();
                TeaSelector.DataSource = _datasource;
                TeaSelector.SelectionChanged += (sender, e) => SelectionChanged();
                // Setup the timer callback
                _timer.AutoReset = true;
                _timer.Elapsed += (sender, e) => TimerLabel.InvokeOnMainThread(UpdateLabel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
        }

        public override NSObject RepresentedObject
        {
            get => base.RepresentedObject;
            // Update the view, if already loaded.
            set => base.RepresentedObject = value;
        }
        #endregion

        #region methods
        [Action("openEditWindow:")]
        public void OpenEditWindow(NSObject sender)
        {
            TeaViewController teaViewController = Storyboard.InstantiateControllerWithIdentifier("TeaViewController") as TeaViewController;
            teaViewController.Tea = _selectedTea;
            PresentViewControllerAsModalWindow(teaViewController);
        }

        [Action("validateMenuItem:")]
        public bool ValidateMenuItem(NSMenuItem item)
        {
            switch (item.Identifier)
            {
                case "EditTeaMenuItem":
                    return _selectedTea != null;
            }
            return true;
        }

        partial void ButtonClicked(NSObject sender)
        {
            _startStopButton = (NSButton)sender;

            if (_timer.Enabled)
            {
                _timer.Stop();
                _startStopButton.Title = "Start";
            }
            else
            {
                _timer.Start();
                _startStopButton.Title = "Stop";
            }
        }

        private void UpdateLabel()
        {
            if (_steepTime.TotalSeconds > 0)
            {

                _steepTime = _steepTime.Subtract(new TimeSpan(0, 0, 1));
                TimerLabel.StringValue = _steepTime.ToString();
            }
            else
            {
                _timer.Stop();
                _startStopButton.Title = "Start";
                TimerLabel.StringValue = "Tea is Ready!";
                _steepTime = _selectedTea.SteepTime;
            }
        }

        private void SelectionChanged()
        {
            _selectedTea = _datasource.Teas[(int)TeaSelector.SelectedIndex];
            _timer.Stop();
            _steepTime = _selectedTea.SteepTime;
            TimerLabel.StringValue = _steepTime.ToString();
        }
        #endregion
    }
}
