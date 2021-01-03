using System;
using System.Collections.Generic;

namespace PetNetCore.Entity
{
    public partial class User
    {
        public User()
        {
            Animal = new HashSet<Animal>();
            BlogPost = new HashSet<BlogPost>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string City { get; set; }
        public decimal Phone { get; set; }
        public int RoleId { get; set; }
        public string Photo { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Animal> Animal { get; set; }
        public virtual ICollection<BlogPost> BlogPost { get; set; }
    }
}
