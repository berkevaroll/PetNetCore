using Microsoft.AspNetCore.Authentication;
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
using System.Threading.Tasks;

namespace PetNetCore.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        PetNETv2Context _context;
        IWebHostEnvironment _env;
        public UserController(PetNETv2Context context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        // GET: UserController
        public IActionResult Index()
        {
            var blogposts = _context.BlogPost.Include(a => a.User).Select(s => new BlogPostDto.List
            {
                BlogTitle = s.BlogTitle,
                BlogContent = s.BlogContent,
                Photo = s.Photo,
                Username = s.User.Username,
            }).ToList();
            return View(blogposts);
        }
        public IActionResult CreateBlogPost(BlogPostDto.List model, IFormFile photo)
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
                string userId = User.Claims.Where(c => c.Type == "UserId").FirstOrDefault().Value;
                var blogPost = new BlogPost
                {
                    BlogTitle = model.BlogTitle,
                    BlogContent = model.BlogContent,
                    Photo = uniqueFileName,
                    UserId = Int32.Parse(userId),
                };
                _context.BlogPost.Add(blogPost);
                _context.SaveChanges();
                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult ListBlogPosts()
        {
            ViewBag.blogposts = _context.BlogPost.Include(m => m.User).Select(s => new BlogPostDto.List
            {
                BlogTitle = s.BlogTitle,
                BlogContent = s.BlogContent,
                Photo = s.Photo,
                Username = s.User.Username,
            }).ToList();
            return View();
        }
        [HttpPost]
        public IActionResult ListBlogPostsByUserId()
        {
            string userId = User.Claims.Where(c => c.Type == "UserId").FirstOrDefault().Value;
            ViewBag.blogposts = _context.BlogPost.Include(m => m.User).Where(m => m.UserId == Int32.Parse(userId)).Select(s => new BlogPostDto.List
            {
                BlogTitle = s.BlogTitle,
                BlogContent = s.BlogContent,
                Photo = s.Photo,
                Username = s.User.Username,
            }).ToList();
            return View("ListBlogPosts");
        }
        public IActionResult Account(int id)
        {
            var user = _context.User.Include(u => u.Role).Where(u => u.Id == id).Select(s => new UserDto.Account
            {
                Id = s.Id,
                Username = s.Username,
                FirstName = s.FirstName,
                LastName = s.LastName,
                BirthDate = s.BirthDate,
                City = s.City,
                Phone = s.Phone,
                Role = s.Role.RoleName,
                Photo = s.Photo,
            }).FirstOrDefault();
            return View(user);
        }

        public IActionResult AddBlogPost()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBlogPost(BlogPostDto.List model, IFormFile photo)
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
                string userId = User.Claims.Where(c => c.Type == "UserId").FirstOrDefault().Value;
                var blogPost = new BlogPost
                {
                    BlogTitle = model.BlogTitle,
                    BlogContent = model.BlogTitle,
                    Photo = model.Photo,
                    UserId = Int32.Parse(userId),
                };
                _context.BlogPost.Add(blogPost);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserDto.Register model, IFormFile photo)
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
            string userId = User.Claims.Where(c => c.Type == "UserId").FirstOrDefault().Value;
            var user = _context.User.Where(m => m.Id == Int32.Parse(userId)).FirstOrDefault();

            user.Username = model.Username;
            user.Password = model.Password;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.BirthDate = (model.BirthDate != null) ? model.BirthDate : user.BirthDate;
            user.City = model.City;
            user.Photo = (photo != null) ? uniqueFileName : user.Photo;
            user.Phone = (model.Phone != null) ? model.Phone : user.Phone;

            _context.User.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Account", new { id = user.Id });
        }
        public IActionResult UpdateProfile()
        {
            string userId = User.Claims.Where(c => c.Type == "UserId").FirstOrDefault().Value;
            var user = _context.User.Where(m => m.Id == Int32.Parse(userId)).Select(s => new UserDto.Register
            {
                Username = s.Username,
                FirstName = s.FirstName,
                LastName = s.LastName,
                BirthDate = s.BirthDate,
                Photo = s.Photo,
                City = s.City,
                Phone = s.Phone,
                Password = s.Password,
            }).FirstOrDefault();
            return View(user);
        }

    }
}
