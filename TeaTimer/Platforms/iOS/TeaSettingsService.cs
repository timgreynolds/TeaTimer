using System;
using Foundation;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public partial class TeaSettingsService : ISettingsService
	{
        public static partial void LoadDefaultSettings() 
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

        public static partial void SettingsChanged() 
        {
            Console.WriteLine("Settings Changed");
        }
    }
}