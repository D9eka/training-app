using System;
using System.Collections.Generic;
using Models;

namespace Data
{
    public interface IDataService<T>
        where T: IModel
    {
        event Action<IReadOnlyList<T>> DataUpdated;
        event Action<string> DataRemoved;

        void ForceUpdate();
        
        IReadOnlyList<T> Cache { get; }

        T GetDataById(string id);

        void AddData(T data);

        void UpdateData(T data);

        void RemoveData(string id);
    }
}