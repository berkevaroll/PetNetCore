using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetNetCore.Entity;
using PetNetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetNetCore.Controllers
{
    [Authorize(Roles = "Administrator")]

    public class AdminController : Controller
    {
        PetNETv2Context _context;
        public AdminController(PetNETv2Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return  RedirectToAction("ManageUser");
        }

        #region Animal Operations
        public IActionResult ManageAnimal()
        {
            ViewBag.animals = _context.Animal.Include(a => a.User).Include(a => a.Breed).Select(s => new AnimalDto
            {
                Name = s.Name,
                Age = s.Age,
                Description = s.Description,
                Photo = s.Photo,
                BreedName = s.Breed.BreedName,
                Username = s.User.Username,
                Id = s.Id,
            }).ToList();
            return View();
        }
        [HttpPost]
        public IActionResult ManageAnimal(AnimalDto model)
        {
            return View();
        }
        public IActionResult AnimalDetails(int? id)
        {
            var animal = _context.Animal.Where(a => a.Id == id).Include(a => a.User).Include(a => a.Breed).Select(s => new AnimalDto
            {
                Name = s.Name,
                Age = s.Age,
                Description = s.Description,
                Photo = s.Photo,
                BreedName = s.Breed.BreedName,
                Username = s.User.Username,
                BreedId = s.BreedId,
            }).FirstOrDefault();
            return View(animal);
        }
        public IActionResult DeleteAnimal(int id)
        {
            var animal = _context.Animal.Where(a => a.Id == id).FirstOrDefault();
            _context.Animal.Remove(animal);
            _context.SaveChanges();
            return RedirectToAction("ManageAnimal");
        }
        [HttpPost]
        public IActionResult UpdateAnimal(AnimalDto model)
        {
            var animal = _context.Animal.Where(a => a.Id == model.Id).FirstOrDefault();
            animal.Name = model.Name;
            animal.Age = model.Age;
            animal.Description = model.Description;
            animal.Photo = model.Photo;
            animal.BreedId = model.BreedId;

            _context.Update(animal);
            _context.SaveChanges();
            return RedirectToAction("ManageAnimal");
        }


        #endregion
        #region User Operations
        public IActionResult ManageUser()
        {
            ViewBag.users = _context.User.Select(s => new User
            {
                Id = s.Id,
                Username = s.Username,
                FirstName = s.FirstName,
                LastName = s.LastName,
                BirthDate = s.BirthDate,
                City = s.City,
                Phone = s.Phone,
            }).ToList();
            return View();
        }
        [HttpPost]
        public IActionResult ManageUser(User model)
        {
            return View();
        }
        public IActionResult UserDetails(int? id)
        {
            var user = _context.User.Where(a => a.Id == id).Select(s => new UserDto.Register
            {
                Username = s.Username,
                FirstName = s.FirstName,
                LastName = s.LastName,
                BirthDate = s.BirthDate,
                City = s.City,
                Phone = s.Phone,
            }).FirstOrDefault();
            return View(user);
        }
        public IActionResult DeleteUser(int id)
        {
            var user = _context.User.Where(a => a.Id == id).FirstOrDefault();
            var animalsOfUser = _context.Animal.Where(a => a.UserId == id).ToList();
            if (animalsOfUser.Count != 0)
            {
                for (int i = 0; i < animalsOfUser.Count; i++)
                {
                    _context.Animal.Remove(animalsOfUser[i]);
                }
            }
            var blogPostsOfUser = _context.BlogPost.Where(a => a.UserId == id).ToList();
            if (blogPostsOfUser.Count != 0)
            {
                for (int i = 0; i < blogPostsOfUser.Count; i++)
                {
                    _context.BlogPost.Remove(blogPostsOfUser[i]);
                }
            }
            _context.User.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("ManageUser");
        }
        #endregion
        #region BlogPost Operations
        public IActionResult ManageBlogPost()
        {
            ViewBag.blogposts = _context.BlogPost.Include(m => m.User).Select(s => new BlogPostDto.List
            {
                Id = s.Id,
                BlogTitle = s.BlogTitle,
                BlogContent = s.BlogContent,
                Photo = s.Photo,
                Username = s.User.Username,
            }).ToList();
            return View();
        }
        [HttpPost]
        public IActionResult ManageBlogPost(BlogPostDto.List model)
        {
            return View();
        }
        public IActionResult BlogPostDetails(int? id)
        {
            var blogPost = _context.BlogPost.Include(c => c.User).Where(a => a.Id == id).Select(s => new BlogPostDto.List

            {
                BlogTitle = s.BlogTitle,
                BlogContent = s.BlogContent,
                Photo = s.Photo,
                Username = s.User.Username,

            }).FirstOrDefault();
            return View(blogPost);
        }
        public IActionResult DeleteBlogPost(int id)
        {
            var blogPost = _context.BlogPost.Where(a => a.Id == id).FirstOrDefault();

            _context.BlogPost.Remove(blogPost);
            _context.SaveChanges();
            return RedirectToAction("ManageBlogPost");
        }
        #endregion
    }
}
