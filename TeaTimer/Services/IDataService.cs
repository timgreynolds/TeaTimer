using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public interface IDataService<T>
    {
        public void Initialize();
        public List<T> Get();
        public Task<List<T>> GetAsync();
        public T FindById(object id);
        public Task<T> FindByIdAsync(object id);
        public T Add(object obj);
        public Task<T> AddAsync(object obj);
        public T Update(object obj);
        public Task<T> UpdateAsync(object obj);
        public bool Delete(object obj);
        public Task<bool> DeleteAsync(object obj);
    }
}