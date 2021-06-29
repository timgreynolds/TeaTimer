using System;
using System.Timers;
using AppKit;
using Foundation;

namespace TeaTimer
{
    public partial class ViewController : NSViewController
    {
        private Timer _timer = new Timer(1000.0);
        private TimeSpan _steepTime;
        private NSButton _startStopButton;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
            // Setup the timer callback
            _timer.AutoReset = true;
            _timer.Elapsed += ElapsedEventHandler;
        }

        partial void ButtonClicked(NSObject sender)
        {
            _startStopButton = (NSButton)sender;

            if (_timer.Enabled)
            {
                _timer.Stop();
                _steepTime = new TimeSpan(0, 0, 30);
                _startStopButton.Title = "Start";
                TimerLabel.StringValue = "Choose a Variety";
            }
            else
            {
                _steepTime = new TimeSpan(0, 0, 30);
                _timer.Start();
                _startStopButton.Title = "Stop";
                TimerLabel.StringValue = _steepTime.ToString();
            }
        }

        public override NSObject RepresentedObject
        {
            get => base.RepresentedObject;
            // Update the view, if already loaded.
            set => base.RepresentedObject = value;
        }

        private void ElapsedEventHandler(object source, ElapsedEventArgs args)
        {
            TimerLabel.InvokeOnMainThread(() => UpdateLabel()) ;
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
            }
        }
    }
}
