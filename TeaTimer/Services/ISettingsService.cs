using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public interface ISettingsService
    {
        public void Clear(string sharedName = null);
        public bool ContainsKey(string key, string sharedName = null);
        public T Get<T>(string key, T defaultValue, string sharedName = null);
        public void Remove(string key, string sharedName = null);
        public void Set<T>(string key, T value, string sharedName = null);
    }
}

