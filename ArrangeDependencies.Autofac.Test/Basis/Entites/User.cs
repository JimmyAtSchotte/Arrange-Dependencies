using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ArrangeDependencies.Autofac.Test.Basis.Entites
{
    public class User
    {
        [Key]
        public int Id { get; private set; }

        public string Name { get; private set; }

        public int CompanyId { get; private set; }

        public virtual Company Company { get; private set; }


        public void SetName(string name) => Name = name;

        public void SetCompany(Company company) => CompanyId = company.Id;
    }
}
