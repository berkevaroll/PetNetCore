using System;
using System.Collections.Generic;

namespace PetNetCore.Entity
{
    public partial class Breed
    {
        public Breed()
        {
            Animal = new HashSet<Animal>();
        }

        public int Id { get; set; }
        public string BreedName { get; set; }

        public virtual ICollection<Animal> Animal { get; set; }
    }
}
