using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public interface IDataService<T>
    {
        public bool Initialize();
        public List<T> Get();
        public Task<List<T>> GetAsync();
        public T FindById(object id);
        public T Add(object obj);
        public T Update(object obj);
        public bool Delete(object obj);
    }
}