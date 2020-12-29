using System;
using System.Collections.Generic;

namespace PetNetCore.Entity
{
    public partial class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public int? BreedId { get; set; }
        public int UserId { get; set; }

        public virtual Breed Breed { get; set; }
        public virtual User User { get; set; }
    }
}
