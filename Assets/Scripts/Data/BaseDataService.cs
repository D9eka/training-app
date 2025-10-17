using System;
using System.Collections.Generic;
using Models;

namespace Data
{
    public abstract class BaseDataService<T> : IDataService<T> 
        where T : IModel
    {
        public event Action<IReadOnlyList<T>> DataUpdated;
        public event Action<string> DataRemoved;

        private List<T> _cache;

        public IReadOnlyList<T> Cache => _cache.AsReadOnly();

        public BaseDataService(List<T> cache)
        {
            _cache = cache;
        }

        public void ForceUpdate()
        {
            DataUpdated?.Invoke(Cache);
        }
        
        public T GetDataById(string id) => _cache.Find(e => e.Id == id);

        public void AddData(T data)
        {
            _cache.Add(data);
            DataUpdated?.Invoke(Cache);
        }

        public void UpdateData(T data)
        {
            int i = _cache.FindIndex(e => e.Id == data.Id);
            if (i >= 0) _cache[i] = data;
            DataUpdated?.Invoke(Cache);
        }

        public void RemoveData(string id)
        {
            T eq = _cache.Find(e => e.Id == id);
            _cache.Remove(eq);
            DataRemoved?.Invoke(id);
            DataUpdated?.Invoke(Cache);
        }
    }
}