using System.Collections.Generic;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public interface IDataService<T>
    {
        public bool Initialize();
        public List<T> Get();
        public T FindById(object id);
        public T Add(object obj);
        public T Update(object obj);
        public bool Delete(object obj);
    }
}