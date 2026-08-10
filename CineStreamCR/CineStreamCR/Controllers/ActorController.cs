using CineStreamCR.BLL.DTO.Actor;
using CineStreamCR.BLL.DTO.Movie;
using CineStreamCR.BLL.Services.Actor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StreamingApp.Models;

namespace CineStreamCR.Controllers
{
    public class ActorController : Controller
    {
        private readonly IActorService _actorService;
        private readonly IMovieActorService _movieActorService;

        public ActorController(IActorService actorService, IMovieActorService movieActorService)
        {
            _actorService = actorService;
            _movieActorService = movieActorService;
        }


        //  VIEWS


        [HttpGet]
        public IActionResult Actors()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var actorResult = await _actorService.GetActorByIdAsync(id);

            if (!actorResult.EsCorrecto || actorResult.Dato == null)
            {
                TempData["Error"] = actorResult.mensaje ?? "Actor not found.";
                return RedirectToAction(nameof(Actors));
            }

            var moviesResult = await _movieActorService.GetMoviesByActorId(id);

            var viewModel = new ActorDetailViewModel
            {
                Actor = actorResult.Dato,

                Movies = moviesResult.EsCorrecto && moviesResult.Dato != null
                    ? moviesResult.Dato
                    : new List<MovieCastMemberDTO>()
            };

            return View(viewModel);
        }




        //  READ (JSON)

        [HttpGet]
        public async Task<IActionResult> GetActors()
        {
            var result = await _actorService.GetAllActorsAsync();
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveActors(byte isActive)
        {
            var result = await _actorService.GetActiveActorsAsync(isActive);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetActorById(int id)
        {
            var result = await _actorService.GetActorByIdAsync(id);
            if (!result.EsCorrecto)
                return NotFound(result);

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetActorByName(string firstName, string lastName)
        {
            var result = await _actorService.GetActorByNameAsync(firstName, lastName);
            if (!result.EsCorrecto)
                return NotFound(result);

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetActorsByMovie(int movieId)
        {
            var result = await _actorService.GetActorsByMovieIdAsync(movieId);
            return Json(result);
        }


        //  CREATE

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
                actorDTO.PictureImg = "/images/actors/" + fileName;
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

        [HttpPost]
        public async Task<IActionResult> EditActor(int id, CreateActorDTO actorDTO, IFormFile? pictureFile)
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
                actorDTO.PictureImg = "/images/actors/" + fileName;
            }

            var result = await _actorService.GetUpdateActorAsync(id, actorDTO);

            if (!result.EsCorrecto)
                return BadRequest(result);

            
            return Json(result);
        }


        //  DELETE

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


        //  ASIGNACIÓN A PELÍCULAS (Endpoints de movieActors)
        [HttpGet]
        public async Task<IActionResult> GetMoviesByActor(int actorId)
        {
            var result = await _movieActorService.GetMoviesByActorId(actorId);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> AssignActorToMovie(AssignActorToMovieDTO dto)
        {
            var result = await _movieActorService.AssignActorToMovie(dto);

            if (!result.EsCorrecto)
                return BadRequest(result);

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCharacterName(int movieId, int actorId, string characterName)
        {
            var result = await _movieActorService.UpdateCharacterName(movieId, actorId, characterName);

            if (!result.EsCorrecto)
                return BadRequest(result);

            return Json(result);
        }

    
        [HttpPost]
        public async Task<IActionResult> RemoveActorFromMovie(int movieId, int actorId)
        {
            var result = await _movieActorService.RemoveActorFromMovie(movieId, actorId);

            if (!result.EsCorrecto)
                return NotFound(result);

            return Json(result);
        }
    }
}