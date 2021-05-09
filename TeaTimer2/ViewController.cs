using System;
using AppKit;
using Foundation;

namespace TeaTimer
{
    public partial class ViewController : NSViewController
    {
        private bool isRunning = false;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
        }

        partial void ButtonClicked(NSObject sender) 
        {
            NSButton startStopButton = (NSButton)sender;

            if (isRunning)
            {
                isRunning = false;
                startStopButton.Title = "Start";
                TimerLabel.StringValue = "Choose a Variety";
            }
            else
            {
                isRunning = true;
                startStopButton.Title = "Stop";
                TimerLabel.StringValue = "Countdown started";
            }
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }
    }
}
