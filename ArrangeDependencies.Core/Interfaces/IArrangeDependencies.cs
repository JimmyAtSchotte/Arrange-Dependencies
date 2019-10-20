using System;
using System.Collections.Generic;
using System.Text;

namespace ArrangeDependencies.Core.Interfaces
{
    public interface IArrangeDependencies
    {
        TInterface Resolve<TInterface>();
    }
}
