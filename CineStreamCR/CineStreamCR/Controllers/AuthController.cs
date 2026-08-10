using CineStreamCR.BLL.DTO.Auth;
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

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(registerDTO);
            }

            var registered = await _authService.RegisterAsync(registerDTO);

            if (!registered)
            {
                ViewBag.Error = "An account with this email already exists.";
                return View(registerDTO);
            }

            TempData["Success"] = "Account created successfully. You can now sign in.";

            return RedirectToAction("Login");
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