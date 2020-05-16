using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWall.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace TheWall.Controllers
{
    public class HomeController : Controller
    {
        private YourContext dbContext;
        
        // here we can "inject" our context service into the constructor
        public HomeController(YourContext context)
        {
            dbContext = context;
        }
        
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            ViewBag.User = dbContext.Users
                .FirstOrDefault(l => l.UserId == (int) HttpContext.Session.GetInt32("UserId"));

            ViewBag.Messages = dbContext.Messages
                .Include(l => l.CreatorOfMessage)
                .Include(l => l.CommentsPostedOnMessage)
                .ThenInclude(y => y.CreatorOfCommment)
                .ToList().OrderByDescending(t => t.CreatedAt);
            return View();
        }

        [Route("NewUser")]
        [HttpPost]
        public IActionResult NewUser(User user)
        {
            // Handle your form submission here
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                dbContext.Add(user);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("UserId", user.UserId);
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("Index");
            }
        }
        [Route("trylogin")]
        [HttpPost]
        public IActionResult Login(LoginUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.Email);
                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Login");
                }
                
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);
                if(result == 0)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Login");
                }
                HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }

        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }     

        [HttpPost]
        [Route("NewMessage")]
        public IActionResult NewMessage(MessageHelper Message)
        {
            if (ModelState.IsValid) {
                Message newMessage = new Message {
                    MessageContent = Message.MessageContent,
                    UserId = (int) HttpContext.Session.GetInt32("UserId")
                };
                dbContext.Add(newMessage);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            } else {
                ViewBag.User = dbContext.Users
                    .FirstOrDefault(l => l.UserId == (int) HttpContext.Session.GetInt32("UserId"));

                ViewBag.Messages = dbContext.Messages
                    .Include(l => l.CreatorOfMessage)
                    .Include(l => l.CommentsPostedOnMessage)
                    .ThenInclude(y => y.CreatorOfCommment)
                    .ToList().OrderByDescending(t => t.CreatedAt);
                return View("Dashboard");
            }

        }

        [HttpPost]
        [Route("NewComment")]
        public IActionResult NewComment(CommentHelper Comment)
        {
            if (ModelState.IsValid) {
                Comment newComment = new Comment {
                    CommentContent = Comment.CommentContent,
                    UserId = (int) HttpContext.Session.GetInt32("UserId"),
                    MessageId = Comment.MessageId
                };
                dbContext.Add(newComment);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            } else {
                ViewBag.User = dbContext.Users
                    .FirstOrDefault(l => l.UserId == (int) HttpContext.Session.GetInt32("UserId"));

                ViewBag.Messages = dbContext.Messages
                    .Include(l => l.CreatorOfMessage)
                    .Include(l => l.CommentsPostedOnMessage)
                    .ThenInclude(y => y.CreatorOfCommment)
                    .ToList().OrderByDescending(t => t.CreatedAt);
                return View("Dashboard");
            }

        }
    }
}
