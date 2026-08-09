using CineStreamCR.BLL.DTO.User;
using CineStreamCR.BLL.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineStreamCR.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        //  VIEWS

        [HttpGet]
        public IActionResult Users()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile(int id)
        {
            var result = await _userService.GetUserById(id);

            if (!result.EsCorrecto)
            {
                TempData["Error"] = result.mensaje ?? "User not found.";
                return RedirectToAction("Login", "Auth");
            }

            return View(result.Dato);
        }


        //  READ (JSON)

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.GetAllUsers();
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetUserById(id);
            if (!result.EsCorrecto)
                return NotFound(result);

            return Json(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var result = await _userService.GetUserByEmail(email);
            if (!result.EsCorrecto)
                return NotFound(result);

            return Json(result);
        }


        //  CREATE (registro público)

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDTO userDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.GetCreateUser(userDTO);

            if (!result.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, result.mensaje ?? "Could not create the user.");
                return BadRequest(result);
            }

            return Json(result);
        }


        //  EDIT

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var result = await _userService.GetUserById(id);

            if (!result.EsCorrecto)
            {
                TempData["Error"] = result.mensaje ?? "User not found.";
                return RedirectToAction(nameof(Profile), new { id });
            }

            return View("~/Views/User/EditUser.cshtml", result.Dato);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(int id, UpdateUserDTO userDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.GetUpdateUser(id, userDTO);

            if (!result.EsCorrecto)
                return BadRequest(result);

            return Json(result);
        }


        //  DELETE

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.GetDeleteUser(id);

            if (!result.EsCorrecto)
                return NotFound(result);

            return Json(result);
        }
    }
}
