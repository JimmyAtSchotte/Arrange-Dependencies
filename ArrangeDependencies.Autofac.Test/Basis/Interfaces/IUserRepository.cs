using ArrangeDependencies.Autofac.Test.Basis.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArrangeDependencies.Autofac.Test.Basis.Interfaces
{
    public interface IUserRepository
    {
        User GetByName(string name);

    }
}
