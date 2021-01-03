using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Authorize]
    public class UserController : Controller
    {
        PetNETv2Context _context;
        public UserController(PetNETv2Context context)
        {
            _context = context;
        }
        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index","Home");
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
            ViewBag.blogs = _context.BlogPost.Include(m => m.User).Where(m=> m.UserId == Int32.Parse(userId)).Select(s =>new BlogPostDto.List
            {
                BlogTitle = s.BlogTitle,
                BlogContent = s.BlogContent,
                Photo = s.Photo,
                Username = s.User.Username,
            }).ToList();
            return View("ListBlogPosts");
        }
    }
}
