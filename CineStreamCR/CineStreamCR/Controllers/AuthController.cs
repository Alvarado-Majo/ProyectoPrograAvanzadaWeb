using CineStreamCR.BLL.DTO.Auth;
using CineStreamCR.BLL.Services.Auth;
using CineStreamCR.BLL.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace StreamingApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return View(loginDTO);

            var user = await _authService.LoginAsync(loginDTO);

            if (user == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View(loginDTO);
            }

            HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetInt32("UserId", user.UserId);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
    }
}