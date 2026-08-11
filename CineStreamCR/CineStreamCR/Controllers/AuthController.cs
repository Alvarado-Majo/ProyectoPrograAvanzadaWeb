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


        // =========================
        // LOGIN - GET
        // =========================

        [HttpGet]
        public IActionResult Login()
        {
            // Si ya hay una sesión iniciada,
            // lo manda directamente al Home.
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                return RedirectToAction(
                    "Index",
                    "Home");
            }

            return View();
        }


        // =========================
        // REGISTER - GET
        // =========================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        // =========================
        // REGISTER - POST
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(registerDTO);
            }

            var registered =
                await _authService.RegisterAsync(
                    registerDTO);

            if (!registered)
            {
                ViewBag.Error =
                    "An account with this email already exists.";

                return View(registerDTO);
            }

            TempData["Success"] =
                "Account created successfully. You can now sign in.";

            return RedirectToAction(
                nameof(Login));
        }


        // =========================
        // LOGIN - POST
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDTO);
            }

            var user =
                await _authService.LoginAsync(
                    loginDTO);

            if (user == null)
            {
                ViewBag.Error =
                    "Correo o contraseña incorrectos.";

                return View(loginDTO);
            }


            // =========================
            // CREAR SESIÓN DEL USUARIO
            // =========================

            HttpContext.Session.SetInt32(
                "UserId",
                user.UserId);

            HttpContext.Session.SetString(
                "UserName",
                $"{user.FirstName} {user.LastName}");

            HttpContext.Session.SetString(
                "UserEmail",
                user.Email);


            // =========================
            // REDIRECCIÓN AL HOME
            // =========================

            return RedirectToAction(
                "Index",
                "Home");
        }


        // =========================
        // LOGOUT
        // =========================

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                nameof(Login));
        }
    }
}