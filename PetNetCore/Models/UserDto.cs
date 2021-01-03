using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetNetCore.Models
{
    public class UserDto
    {
        public class Login
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
        public class Register
        {

            public string Username { get; set; }
            public string Password { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime? BirthDate { get; set; }
            public string City { get; set; }
            public string Photo { get; set; }
            public decimal Phone { get; set; }
        }
        public class Account
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime? BirthDate { get; set; }
            public string City { get; set; }
            public decimal Phone { get; set; }
            public string Role { get; set; }
            public string Photo { get; set; }
        }
    }
}
