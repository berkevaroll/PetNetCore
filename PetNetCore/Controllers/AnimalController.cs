using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetNetCore.Entity;
using PetNetCore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PetNetCore.Controllers
{
    [Authorize]
    public class AnimalController : Controller
    {
        PetNETv2Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;
        public AnimalController(PetNETv2Context context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _env = env;
        }
        // GET: AnimalController
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult List()
        {
            ViewBag.animals = _context.Animal.Include(a => a.User).Include(a => a.Breed).Select(s => new AnimalDto
            {
                Name = s.Name,
                Age = s.Age,
                Description = s.Description,
                Photo = s.Photo,
                BreedName = s.Breed.BreedName,
                Username = s.User.Username,
            }).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult ListByUserId()
        {
            string userId = User.Claims.Where(c => c.Type == "UserId").FirstOrDefault().Value;
            ViewBag.animals = _context.Animal.Where(m=> m.UserId == Int32.Parse(userId)).Select(s => new AnimalDto
            {
                Name = s.Name,
                Age = s.Age,
                Description = s.Description,
                Photo = s.Photo,
                BreedName = s.Breed.BreedName,
                Username = s.User.Username,
            }).ToList();
            return View("List");
        }
        public ActionResult Create()
        {
            return View();
        }
        //POST: AnimalController/Create
       [HttpPost]
        public ActionResult Create(AnimalDto animalModel, IFormFile photo)
        {
            try
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
                string userId = User.Claims.Where(c=>c.Type == "UserId").FirstOrDefault().Value;
                var animal = new Animal
                {
                    Name = animalModel.Name,
                    Age = animalModel.Age,
                    Description = animalModel.Description,
                    Photo = uniqueFileName,
                    BreedId = animalModel.BreedId,
                    UserId = Int32.Parse(userId),
                };
                _context.Animal.Add(animal);
                _context.SaveChanges();
                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }

    }
}
