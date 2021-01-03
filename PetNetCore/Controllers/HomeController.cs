using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetNetCore.Entity;
using PetNetCore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace PetNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _env;
        public PetNETv2Context _context;
        public HomeController(ILogger<HomeController> logger, PetNETv2Context context, IHttpContextAccessor httpContextAccessor, IHostingEnvironment env)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _env = env;
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "User");
            }
            return View();
        }
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserDto.Login model)
        {
            var user = _context.User.Where(m => m.Username == model.Username && m.Password == model.Password).FirstOrDefault();
            if (user != null)
            {
                string role;
                if (user.RoleId == 1) { role = "User"; }
                else if (user.RoleId == 2) { role = "Administrator"; }
                else { role = "Blogger"; }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim("UserId", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, role),

                };
                var claimsIdentity = new ClaimsIdentity(claims, "Login");
                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return Redirect("/User");
            }
            return RedirectToAction("Index");
        }
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserDto.Register model, IFormFile photo)
        {
            string uniqueFileName = null;
            if (photo != null)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }
            }

            var user = new User
            {
                Username = model.Username,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
                City = model.City,
                Photo = uniqueFileName,
                Phone = model.Phone,
                RoleId = 1,

            };
            _context.User.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Login", new UserDto.Login
            {
                Username = model.Username,
                Password = model.Password
            });
        }
    }
}
