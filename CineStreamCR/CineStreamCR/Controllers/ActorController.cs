using CineStreamCR.BLL.DTO.Actor;
using CineStreamCR.BLL.Services.Actor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineStreamCR.Controllers
{
    public class ActorController : Controller
    {
        private readonly IActorService _actorService;

        public ActorController(IActorService actorService)
        {
            _actorService = actorService;
        }


        //  VIEWS


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Actors()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult RegisterActor()
        {
            return View();
        }


        //  READ (JSON)


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetActors()
        {
            var result = await _actorService.GetAllActorsAsync();
            return Json(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetActiveActors(byte isActive)
        {
            var result = await _actorService.GetActiveActorsAsync(isActive);
            return Json(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> GetActorById(int id)
        {
            var result = await _actorService.GetActorByIdAsync(id);
            if (!result.EsCorrecto)
                return NotFound(result);

            return Json(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> GetActorByName(string firstName, string lastName)
        {
            var result = await _actorService.GetActorByNameAsync(firstName, lastName);
            if (!result.EsCorrecto)
                return NotFound(result);

            return Json(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetActorsByMovie(int movieId)
        {
            var result = await _actorService.GetActorsByMovieIdAsync(movieId);
            return Json(result);
        }

  
        //  CREATE


        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> CreateActor(CreateActorDTO actorDTO, IFormFile? pictureFile)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (pictureFile != null && pictureFile.Length > 0)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "actors");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(pictureFile.FileName);
                var fullPath = Path.Combine(folder, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await pictureFile.CopyToAsync(stream);
                actorDTO.PinctureImg = "/images/actors/" + fileName;
            }

            var result = await _actorService.GetCreateActorAsync(actorDTO);

            if (!result.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, result.mensaje ?? "Could not create the actor.");
                return BadRequest(result);
            }

            return Json(result);
        }


        //  EDIT


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditActor(int id)
        {
            var result = await _actorService.GetActorByIdAsync(id);

            if (!result.EsCorrecto)
            {
                TempData["Error"] = result.mensaje ?? "Actor not found.";
                return RedirectToAction(nameof(Actors));
            }

            return View("~/Views/Actor/EditActor.cshtml", result.Dato);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditActor(int id, CreateActorDTO actorDTO, IFormFile? pictureFile)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please complete all required fields.";
                return View(actorDTO);
            }

            if (pictureFile != null && pictureFile.Length > 0)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "actors");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(pictureFile.FileName);
                var fullPath = Path.Combine(folder, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await pictureFile.CopyToAsync(stream);
                actorDTO.PinctureImg = "/images/actors/" + fileName;
            }

            var result = await _actorService.GetUpdateActorAsync(id, actorDTO);

            if (!result.EsCorrecto)
                return BadRequest(result);

            TempData["Success"] = result.mensaje;
            return RedirectToAction(nameof(Actors));
        }

     
        //  DELETE


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteActor(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _actorService.GetDeleteActorAsync(id);

            if (!result.EsCorrecto)
                return NotFound(result);

            return Json(result);
        }
    }
}