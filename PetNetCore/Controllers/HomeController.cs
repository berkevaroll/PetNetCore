using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetNetCore.Entity;
using PetNetCore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PetNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public PetNETv2Context _db;
        public HomeController(ILogger<HomeController> logger, PetNETv2Context db)
        {
            _logger = logger;
            _db = db;
        }
        //Index
        public IActionResult Index()
        {
            return View();
        }

    }
}
