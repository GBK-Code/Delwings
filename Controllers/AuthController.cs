using Delwings.Models.Requests;
using Delwings.Models.Results;
using Delwings.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;


namespace Delwings.Controllers
{
    public class AuthController : Controller
    {
        private readonly LoginService _loginService;
        private readonly AccountRegistrationService _accountRegistrationService;

        public AuthController (LoginService loginService, AccountRegistrationService accountRegistrationService) 
        { 
            _loginService = loginService;
            _accountRegistrationService = accountRegistrationService;
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
            LoginResult? loginResult = await _loginService.LoginPrincipal(login, password);
            if (loginResult == null) { return RedirectToAction("AccessDenied", "Home"); }

            await HttpContext.SignInAsync("Cookies", loginResult.Principal!);

            return RedirectToAction("Dashboard", loginResult.UserRoleString, new { tab = "overview" });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(CreateAccountRequest request)
        {
            await _accountRegistrationService.RegisterUserAsync(request);
            if (request.Login == null || request.Password == null) { return RedirectToAction("BadReq", "Home"); }
            return await Login(request.Login, request.Password);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCourier(CreateAccountRequest request)
        {
            await _accountRegistrationService.RegisterCourierAsync(request);

           if (request.Login == null || request.Password == null) { return RedirectToAction("BadReq", "Home"); }
            return await Login(request.Login, request.Password);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login", "Auth");
        }
    }
}
