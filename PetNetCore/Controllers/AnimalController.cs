using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetNetCore.Entity;
using PetNetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetNetCore.Controllers
{
    public class AnimalController : Controller
    {
        PetNETv2Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
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
        public ActionResult Create()
        {
            return View();
        }
        // POST: AnimalController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AnimalDto animalModel)
        {
            try
            {
                var animal = new Animal
                {
                    Name = animalModel.Name,
                    Age = animalModel.Age,
                    Description = animalModel.Description,
                    BreedId = animalModel.BreedId,
                    UserId = (int)_session.GetInt32("UserId"),
                };
                _context.Animal.Add(animal);
                _context.SaveChanges();
                return View("List");
            }
            catch
            {
                return View();
            }
        }

    }
}
