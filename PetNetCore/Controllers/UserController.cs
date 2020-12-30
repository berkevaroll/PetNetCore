using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetNetCore.Entity;
using PetNetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetNetCore.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        public PetNETv2Context _db;
        public UserController(ILogger<UserController> logger, PetNETv2Context db)
        {
            _logger = logger;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(UserDto model)
        {

            try
            {
                var user = _db.User.Where(u => u.Username == model.Username && u.Password == model.Password).FirstOrDefault();
                if (user != null)
                {
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    if (user.RoleId == 1)
                    {
                        
                        return View("UserLogin");
                    }
                    return View("AdminLogin");
                }
                else
                {
                    return Index();
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult Register(UserDto model)
        {
            User user = new User
            {
                Username = model.Username,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
                City = model.City,
                Phone = model.Phone,
                RoleId = 1,
            };
            _db.User.Add(user);
            _db.SaveChanges();
            return View("Index");
        }
        //Get Register Page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}
