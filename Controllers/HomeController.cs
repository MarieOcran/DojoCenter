using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FinalExam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FinalExam.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context { get; set; } 
        private PasswordHasher<User> regHasher = new PasswordHasher<User>();
        private PasswordHasher<Login> logHasher = new PasswordHasher<Login>();

        
        public User GetUser()
        {
            return _context.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId"));
        }
        public HomeController(MyContext context)
        {
            _context = context;
        }
        


        [HttpGet("")]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpGet("home")]
        public IActionResult Home()
        {
            User current = GetUser();
            if(current == null)
            {
                return Redirect("/");
            }
            ViewBag.User = current;
            List<ActivityCenter> All = _context.Activities
                                            .Include(m => m.Coordinator)
                                            .Include(m => m.Participants)
                                            .ThenInclude(wp => wp.Host)
                                            .Where(m => m.Date >= DateTime.Now)
                                            .OrderBy(m => m.Date)
                                            .ToList();
            return View (All);
        }


        
        [HttpGet("activity/{activityId}")]
        public IActionResult Display(int activityId)
        {
            User current = GetUser();
            if (current == null)
            {
                return Redirect("/");
            }
            ActivityCenter showing = _context.Activities
                                    .Include(m => m.Participants)
                                    .ThenInclude(w => w.Host) 
                                    .Include( m => m.Coordinator)
                                    .FirstOrDefault(m => m.ActivityId == activityId);


            return View(showing);
        }


         [HttpGet("activity/plan")]
        public IActionResult Plan()
        {
            User current = GetUser();
            if (current == null)
            {
                return Redirect ("/");
            }
            return View();
        }


        [HttpPost("activity/plan/add")]
        public IActionResult Create(ActivityCenter newActivity)
        {
            User current = GetUser();
            if (current == null)
            {
                return Redirect ("/");
            }
            if (ModelState.IsValid)
            {
                newActivity.UserId = current.UserId;
                _context.Activities.Add(newActivity);
                _context.SaveChanges();
                return RedirectToAction("Home");
            }
            return View("Plan");
        }
        
        [HttpGet("activity/{activityId}/{status}")]
        public IActionResult ToggleGroup(int activityId, string status)
        {
            User current = GetUser();
            if (current == null)
            {
                return Redirect ("/");
            }
            if(status == "join")
            {
                Group newGroup = new Group();
                newGroup.UserId= current.UserId;
                newGroup.ActivityId = activityId;
                _context.Groups.Add(newGroup);
            }
            else if(status == "leave")
            {
                Group backout = _context.Groups.FirstOrDefault( w => w.UserId == current.UserId && w.ActivityId == activityId);
                _context.Groups.Remove(backout);
            }
            _context.SaveChanges();
            return RedirectToAction("Home");

        }

        [HttpGet("activity/{activityId}/delete")]
        public IActionResult Delete(int activityId)
        {
            User current = GetUser();
            if (current == null)
            {
                return Redirect ("/");
            }
            ActivityCenter remove = _context.Activities.FirstOrDefault(m => m.ActivityId == activityId);
            _context.Activities.Remove(remove);
            _context.SaveChanges();
            return RedirectToAction("Home");

        }


        [HttpGet("activity/{activityId}/edit")]
        public IActionResult Edit(int activityId)
        {
            User current = GetUser();
            if (current == null)
            {
                return Redirect ("Home");
            }
            ActivityCenter edit = _context.Activities.FirstOrDefault(nd => nd.ActivityId == activityId);
            return View(edit);

        }


        [HttpPost("activity/{activityId}/update")]
        public IActionResult Change(int activityId, ActivityCenter Nd)
        {
            if (ModelState.IsValid)
            {
                ActivityCenter edit = _context.Activities.FirstOrDefault(nd => nd.ActivityId == activityId);
                edit.Title = Nd.Title;
                edit.Date = Nd.Date;
                edit.Time = Nd.Time;
                edit.Duration = Nd.Duration;
                edit.Description = Nd.Description;
                
               
                edit.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                return Redirect("Home");
            }
            else
            {   
                
                return View("Edit", Nd);
            }
        }





        [HttpPost("register")]
        public IActionResult Register(User u)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.FirstOrDefault(usr => usr.Email == u.Email) != null)
                {
                    ModelState.AddModelError("Email", "Email already in use, try logging in!");
                    return View("Index");
                }
                string hash = regHasher.HashPassword(u, u.Password);
                u.Password = hash;
                _context.Users.Add(u);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("userId", u.UserId);
                return Redirect("/home");
            }
            return View("Index");
        }

        [HttpPost("login")]
         public IActionResult Login(Login lu)
         {
            if(ModelState.IsValid)
            {
                User userInDB = _context.Users.FirstOrDefault(u => u.Email == lu.EmailLog );
                if(userInDB == null)
                {
                    ModelState.AddModelError("EmailLog", "Invalid Email or Password");
                    return View("Index");

                } 
                var result = logHasher.VerifyHashedPassword(lu, userInDB.Password, lu.PasswordLog);
                if(result == 0)
                {
                    ModelState.AddModelError("PasswordLog", "Invalid Email or Password");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("userId", userInDB.UserId);
                return Redirect("/home");
        
                }
                return View("Index");
            }

        
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }





        public IActionResult Privacy()
        {
            return View();
        }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        // }
    }
}
