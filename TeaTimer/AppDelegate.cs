using System;
using System.Collections.Generic;
using System.IO;
using AppKit;
using Foundation;
using Newtonsoft.Json;

namespace TeaTimer
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        private List<TeaModel> _teas;

        public List<TeaModel> Teas
        {
            get => _teas;
            set => _teas = value;
        }

        public AppDelegate()
        {
        }

        public override void WillFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application
            try
            {
                _teas = TeaVarietiesDataSource.InitializeDatabase();
            }
            catch (IOException ioException)
            {
                NSAlert alert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Critical,
                    MessageText = "An error occurred trying to open the tea varieties database.",
                    InformativeText = $"Message: {ioException.Message}"
                };
                alert.RunModal();
            }
            catch (JsonSerializationException serializationException)
            {
                NSAlert alert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Critical,
                    MessageText = "An error occurred trying to load the list of tea varieties.",
                    InformativeText = $"Message: {serializationException.Message}"
                };
                alert.RunModal();
            }
            catch (Exception exception)
            {
                NSAlert alert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Critical,
                    MessageText = "An error occurred.",
                    InformativeText = $"Message: {exception.Message}"
                };
                alert.RunModal();
            }
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
