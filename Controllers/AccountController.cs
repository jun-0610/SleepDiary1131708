using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SleepDiary1131708.Models;

namespace SleepDiary1131708.Controllers
{
    public class AccountController : Controller
    {
        private readonly SleepDiaryDbContext _context;

        public AccountController(SleepDiaryDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return Redirect("/index.html?error=1");
            }

            var user = _context.Users.FirstOrDefault(u => u.Account == username && u.Password == password);
            if (user == null)
            {
                return Redirect("/index.html?error=1");
            }

            HttpContext.Session.SetInt32("UserID", user.UserID);
            HttpContext.Session.SetString("Account", user.Account);
            HttpContext.Session.SetString("Name", user.Name);

            return RedirectToAction("Index", "SleepLog");
        }

        [HttpPost]
        public IActionResult Register(string username, string password, string fullName)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(fullName))
            {
                return Redirect("/index.html?error=reg_empty");
            }

            var existingUser = _context.Users.FirstOrDefault(u => u.Account == username);
            if (existingUser != null)
            {
                return Redirect("/index.html?error=reg_exists");
            }

            var newUser = new User
            {
                Account = username,
                Password = password,
                Name = fullName
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("UserID", newUser.UserID);
            HttpContext.Session.SetString("Account", newUser.Account);
            HttpContext.Session.SetString("Name", newUser.Name);

            return RedirectToAction("Index", "SleepLog");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/index.html");
        }
    }
}
