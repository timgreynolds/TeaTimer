using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public class TeaSettingsService : ISettingsService
    {
        private static string _sharedName = Assembly.GetExecutingAssembly().GetName().Name;
        
        public TeaSettingsService()
        {
        }

        //public static T Initialize<T>(string key, T defaultValue) => Preferences.Default.Get<T>(key, defaultValue, _sharedName);

        public void Clear() => Preferences.Default.Clear(_sharedName);
        public void Clear(string sharedName) => Preferences.Default.Clear(sharedName);

        public bool ContainsKey(string key) => Preferences.Default.ContainsKey(key, _sharedName);
        public bool ContainsKey(string key, string sharedName) => Preferences.Default.ContainsKey(key, sharedName);

        public T Get<T>(string key, T defaultValue) => Preferences.Default.Get<T>(key, defaultValue, _sharedName);
        public T Get<T>(string key, T defaultValue, string sharedName) => Preferences.Default.Get<T>(key, defaultValue, sharedName);

        public void Remove(string key) => Preferences.Default.Remove(key, _sharedName);
        public void Remove(string key, string sharedName) => Preferences.Default.Remove(key, sharedName);

        public void Set<T>(string key, T value) => Preferences.Default.Set<T>(key, value, _sharedName);
        public void Set<T>(string key, T value, string sharedName) => Preferences.Default.Set<T>(key, value, sharedName);
    }
}

