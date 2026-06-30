using CineStreamCR.BLL.DTO.Director;
using CineStreamCR.BLL.Services.Director;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineStreamCR.Controllers
{
    public class DirectorController : Controller
    {
        private readonly IDirectorService _directorService;

        public DirectorController(IDirectorService directorService)
        {
            _directorService = directorService;
        }


        //  VIEWS


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Directors()
        {
            return View("~/Views/Director/Directors.cshtml");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult RegisterDirector()
        {
            return View("~/Views/Director/RegisterDirector.cshtml");
        }


        //  READ (JSON)
    

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetDirectors()
        {
            var result = await _directorService.GetAllDirectorsAsync();
            return Json(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetActiveDirectors(byte isActive)
        {
            var result = await _directorService.GetActiveDirectorsAsync(isActive);
            return Json(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetDirectorById(int id)
        {
            var result = await _directorService.GetDirectorByIdAsync(id);
            if (!result.EsCorrecto)
                return NotFound(result);

            return Json(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetDirectorByName(string firstName, string lastName)
        {
            var result = await _directorService.GetDirectorByNameAsync(firstName, lastName);
            if (!result.EsCorrecto)
                return NotFound(result);

            return Json(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetDirectorsByMovie(int movieId)
        {
            var result = await _directorService.GetDirectorsByMovieIdAsync(movieId);
            return Json(result);
        }


        //  CREATE


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateDirector(CreateDirectorDTO directorDTO, IFormFile? pictureFile)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (pictureFile != null && pictureFile.Length > 0)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "directors");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(pictureFile.FileName);
                var fullPath = Path.Combine(folder, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await pictureFile.CopyToAsync(stream);
                directorDTO.PinctureImg = "/images/directors/" + fileName;
            }

            var result = await _directorService.GetCreateDirectorAsync(directorDTO);

            if (!result.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, result.mensaje ?? "Could not create the director.");
                return BadRequest(result);
            }

            return Json(result);
        }


        //  EDIT


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditDirector(int id)
        {
            var result = await _directorService.GetDirectorByIdAsync(id);

            if (!result.EsCorrecto)
            {
                TempData["Error"] = result.mensaje ?? "Director not found.";
                return RedirectToAction(nameof(Directors));
            }

            return View("~/Views/Director/EditDirector.cshtml", result.Dato);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditDirector(int id, CreateDirectorDTO directorDTO, IFormFile? pictureFile)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please complete all required fields.";
                return View(directorDTO);
            }

            if (pictureFile != null && pictureFile.Length > 0)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "directors");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(pictureFile.FileName);
                var fullPath = Path.Combine(folder, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await pictureFile.CopyToAsync(stream);
                directorDTO.PinctureImg = "/images/directors/" + fileName;
            }

            var result = await _directorService.GetUpdateDirectorAsync(id, directorDTO);

            if (!result.EsCorrecto)
                return BadRequest(result);

            TempData["Success"] = result.mensaje;
            return RedirectToAction(nameof(Directors));
        }

     
        //  DELETE
        

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteDirector(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _directorService.GetDeleteDirectorAsync(id);

            if (!result.EsCorrecto)
                return NotFound(result);

            return Json(result);
        }
    }
}