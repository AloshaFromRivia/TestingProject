using System;
using System.Collections.Generic;

namespace TestingApp.WebApi.Entities
{
    public interface IRepository<T> : IDisposable where T : IEntity
    {
        IEnumerable<T> Items { get;}
        T GetById(Guid id);
        T Add(T item);
        T Remove(Guid id);
        bool Exist(Guid id);
    }
}