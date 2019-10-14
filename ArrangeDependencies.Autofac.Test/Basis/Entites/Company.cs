using System.ComponentModel.DataAnnotations;

namespace ArrangeDependencies.Autofac.Test.Basis.Entites
{
    public class Company
    {
        [Key]
        public int Id { get; private set; }

        public string Name { get; private set; }


        public void SetName(string name) => Name = name;
    }
}
