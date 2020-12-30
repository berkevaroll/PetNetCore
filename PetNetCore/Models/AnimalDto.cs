using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetNetCore.Models
{
    public class AnimalDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public int? BreedId { get; set; }
        public int UserId { get; set; }
    }
}
