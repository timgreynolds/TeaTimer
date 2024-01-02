using System;
using System.Collections;
using System.Reflection;
using Foundation;
using Log = CoreFoundation.OSLog;
using LogLevel = CoreFoundation.OSLogLevel;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public partial class TeaSettingsService : ISettingsService
	{
        Log _logger = new Log(Assembly.GetExecutingAssembly().GetName().Name, "iOS");

        public partial void LoadDefaultSettings() 
        {
            NSMutableDictionary settings = new NSMutableDictionary(NSBundle.MainBundle.PathForResource("Settings.bundle/Root.plist", null));
            NSArray preferences = settings["PreferenceSpecifiers"] as NSArray;
            foreach (NSDictionary preference in preferences)
            {
                NSString key = (NSString)preference["Key"];
                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }
                if (string.Equals(key, "useCelsiusKey", StringComparison.OrdinalIgnoreCase)) 
                {
                    var defaulValue = preference["DefaultValue"];
                }
            }
        }

        public partial void SettingsChanged() 
        {
             _logger.Log(LogLevel.Fault, $"{nameof(SettingsChanged)} not implemented.");
        }
    }
}