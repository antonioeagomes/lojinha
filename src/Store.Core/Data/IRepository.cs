using Store.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfwork UnitOfwork { get; }
    }
}
