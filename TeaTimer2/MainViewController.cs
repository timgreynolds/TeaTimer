using System;
using System.Timers;
using AppKit;
using Foundation;

namespace TeaTimer
{
    public partial class ViewController : NSViewController
    {
        private static readonly Timer timer = new Timer(1000.0);
        private TimeSpan steepTime;
        private NSButton startStopButton;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
            // Setup the timer callback
            timer.AutoReset = true;
            timer.Elapsed += ElapsedEventHandler;
        }

        partial void ButtonClicked(NSObject sender)
        {
            startStopButton = (NSButton)sender;

            if (timer.Enabled)
            {
                timer.Stop();
                steepTime = new TimeSpan(0, 0, 30);
                startStopButton.Title = "Start";
                TimerLabel.StringValue = "Choose a Variety";
            }
            else
            {
                steepTime = new TimeSpan(0, 0, 30);
                timer.Start();
                startStopButton.Title = "Stop";
                TimerLabel.StringValue = steepTime.ToString();
            }
        }

        public override NSObject RepresentedObject
        {
            get => base.RepresentedObject;
            set => base.RepresentedObject = value;// Update the view, if already loaded.
        }

        private void ElapsedEventHandler(object source, ElapsedEventArgs args)
        {
            TimerLabel.InvokeOnMainThread(() => UpdateLabel()) ;
        }

        private void UpdateLabel()
        {
            if (steepTime.TotalSeconds > 0)
            {

                steepTime = steepTime.Subtract(new TimeSpan(0, 0, 1));
                TimerLabel.StringValue = steepTime.ToString();
            }
            else
            {
                timer.Stop();
                startStopButton.Title = "Start";
                TimerLabel.StringValue = "Tea is Ready!";
            }

        }
    }
}
