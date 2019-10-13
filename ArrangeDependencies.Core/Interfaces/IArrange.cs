using System;
using System.Collections.Generic;
using System.Text;

namespace ArrangeDependencies.Core.Interfaces
{
    public interface IArrange<T> where T : class
    {
        T Resolve();
        TInterface ResolveDependency<TInterface>();
    }
}
