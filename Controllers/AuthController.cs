using Delwings.Models;
using Delwings.Models.Enums;
using Delwings.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Delwings.Controllers
{
    public class AuthController : Controller
    {
        private readonly AccountService _accountService;
        private readonly CourierApplicationService _courierApplicationService;

        public AuthController(AccountService accountService, CourierApplicationService courierApplicationService) { 
            _accountService = accountService;
            _courierApplicationService = courierApplicationService;
        }

        public IActionResult Login()
        {
            return View();
        }
        
        public IActionResult CourierLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string login, string password)
        {
            var user = await _accountService.GetAccountByLoginAsync(login);
            bool validity = await _accountService.CheckValidity(user, password);

            if (validity == false) { return RedirectToAction("AccessDenied", "Home"); }

            string userRole = user.Role.ToString();

            var claims = new List<Claim> 
            { 
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, userRole)
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("Cookies", principal);

            return RedirectToAction("Dashboard", userRole, new { tab = "overview" });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(
                string name, 
                string surname, 
                string login, 
                string phone,
                string email,
                string password,
                string passwordConfirm
            )
        {
            if (password != passwordConfirm)
            {
                return View("Login");
            }

            Account newAccount = new Account
            {
                Name = name,
                Surname = surname,
                Login = login,
                Phone = phone,
                Email = email,
                Password = password,
                Role = Models.Enums.AccountRoles.User
            };

            await _accountService.CreateAccountAsync(newAccount);
            return await Login(newAccount.Login, newAccount.Password);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCourier(
                string name,
                string surname,
                string login,
                string phone,
                string email,
                string password,
                string passwordConfirm
            )
        {
            if (password != passwordConfirm)
            {
                return View("CourierLogin");
            }

            Account newAccount = new Account
            {
                Name = name,
                Surname = surname,
                Login = login,
                Phone = phone,
                Email = email,
                Password = password,
                Role = Models.Enums.AccountRoles.Courier
            };

            await _accountService.CreateAccountAsync(newAccount);
            await _courierApplicationService.CreateApplicationAsync(
                new CourierApplication
                {
                    Name = newAccount.Name,
                    CourierId = newAccount.Id,
                    Surname =newAccount.Surname,
                    Phone = newAccount.Phone,
                    Status = CourierStatuses.Pending
                }    
            );
            return await Login(newAccount.Login, newAccount.Password);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login", "Auth");
        }
    }
}
