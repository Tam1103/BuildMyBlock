using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MyBlock.Models;
using MyBlock.Security;

namespace MyBlock.Controllers
{
    [Route("customer")]
    public class CustomerController : Controller
    {
        private readonly DatabaseContext db;
        private SecurityManager securityManager = new SecurityManager();

        public CustomerController(DatabaseContext _db)
        {
            db = _db;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string username, string password)
        {
            var account = processLogin(username, password);
            if (account != null)
            {
                securityManager.SignIn(this.HttpContext, account);
                return RedirectToAction("index", "home");
            }
            else
            {
                ViewBag.error = "Invalid Account";
                return View("login");
            }
        }

        private Account processLogin(string username, string password)
        {
            var account = db.Accounts.Where(a => a.Username.Equals(username) && a.Status == true);
            foreach (var acc in account)
            {
                if (acc != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, acc.Password))
                    {
                        return acc;
                    }
                }
            }
            return null;
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            var account = new Account();
            return View("Register", account);
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Regester(Account account)
        {
            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            account.Status = true;
            db.Accounts.Add(account);
            db.SaveChanges();

            var roleAccount = new RoleAccount();
            roleAccount.RoleId = 2;
            roleAccount.AccountId = account.Id;
            roleAccount.Status = true;
            db.RoleAccounts.Add(roleAccount);
            db.SaveChanges();
            return RedirectToAction("login", "customer");
        }

        [Route("signout")]
        public IActionResult SignOut()
        {
            securityManager.SignOut(this.HttpContext);
            return RedirectToAction("login", "customer");
        }


        [HttpGet]
        [Route("profile")]
        public IActionResult Profile()
        {
            var user = User.FindFirst(ClaimTypes.Name);
            var username = user.Value;
            var account = db.Accounts.FirstOrDefault(a => a.FullName.Equals(username));
            return View("Profile", account);
        }

        [HttpPost]
        [Route("profile")]
        public IActionResult Profile(Account account)
        {
            var currentAccount = db.Accounts.SingleOrDefault(a => a.Id.Equals(account.Id));
            if (!string.IsNullOrEmpty(account.Password))
            {
                currentAccount.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            }
            currentAccount.Username = account.Username;
            currentAccount.Email = account.Email;
            currentAccount.FullName = account.FullName;
            currentAccount.Status = account.Status;

            ViewBag.msg = "Update successful";

            db.SaveChanges();
            return View("Profile");
        }

        [Route("accessdenied")]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }


        [HttpGet]
        [Route("forgotpassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        [Route("forgotpassword")]
        public IActionResult ForgotPassword(Account account)
        {
            return RedirectToAction("login", "customer");
        }
    }
}