using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetNetCore.Entity;
using PetNetCore.Models;
using System;
using System.Collections.Generic;
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
        public AnimalController(PetNETv2Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        // GET: AnimalController
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult List()
        {
            ViewBag.animals = _context.Animal.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult ListByUserId()
        {
            string userId = User.Claims.Where(c => c.Type == "UserId").FirstOrDefault().Value;
            ViewBag.animals = _context.Animal.Where(m=> m.UserId == Int32.Parse(userId)).ToList();
            return View("List");
        }
        public ActionResult Create()
        {
            return View();
        }
        //POST: AnimalController/Create
       [HttpPost]
        public ActionResult Create(AnimalDto animalModel)
        {
            try
            {
                string userId = User.Claims.Where(c=>c.Type == "UserId").FirstOrDefault().Value;
                var animal = new Animal
                {
                    Name = animalModel.Name,
                    Age = animalModel.Age,
                    Description = animalModel.Description,
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
