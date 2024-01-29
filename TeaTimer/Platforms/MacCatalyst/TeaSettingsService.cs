using System;
using System.Linq;
using System.Reflection;
using com.mahonkin.tim.maui.TeaTimer.ViewModels;
using Foundation;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public partial class TeaSettingsService : ISettingsService
    {
        public static partial void LoadDefaultSettings()
        {
            NSMutableDictionary settings = new NSMutableDictionary(NSBundle.MainBundle.PathForResource("Settings.bundle/Root.plist", null));
            NSArray preferences = settings["PreferenceSpecifiers"] as NSArray;
            NSDictionary useCelsiusPref = preferences.ToArray<NSDictionary>().Where(dict => String.Equals((NSString)dict["Key"], "useCelsiusKey", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            
            bool defaultValue = false;
            if (useCelsiusPref.TryGetValue(NSObject.FromObject("DefaultValue"), out NSObject val))
            {
                Boolean.TryParse(val.ToString(), out defaultValue);

            }
        }

        public static partial void SettingsChanged()
        {
            if (AppShell.Current is not null)
            {
                Page currentPage = AppShell.Current.CurrentPage;
                Type contextType = currentPage.BindingContext.GetType();

                if (contextType.IsAssignableTo(typeof(BaseViewModel)))
                {
                    FieldInfo displayService = contextType.GetRuntimeField("DisplayService`");
                }
            }
        }
    }
}