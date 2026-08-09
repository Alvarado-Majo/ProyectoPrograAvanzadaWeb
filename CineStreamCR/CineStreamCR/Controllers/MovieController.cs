using CineStreamCR.BLL.DTO.Movie;
using CineStreamCR.BLL.Services.Movie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineStreamCR.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }


        //  VIEWS


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Movies()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var result = await _movieService.GetMovieById(id);

            if (!result.EsCorrecto)
            {
                TempData["Error"] = result.mensaje ?? "Movie not found.";
                return RedirectToAction(nameof(Movies));
            }

            return View(result.Dato);
        }


        //  READ (JSON)



        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var result = await _movieService.GetAllMovies();
            return Json(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var result = await _movieService.GetMovieById(id);
            if (!result.EsCorrecto)
                return NotFound(result);

            return Json(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetMovieByTitle(string title)
        {
            var result = await _movieService.GetMovieByTitle(title);
            if (!result.EsCorrecto)
                return NotFound(result);

            return Json(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetMoviesByDirector(int directorId)
        {
            var result = await _movieService.GetMoviesByDirectorId(directorId);
            return Json(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetMoviesByActor(int actorId)
        {
            var result = await _movieService.GetMoviesByActorId(actorId);
            return Json(result);
        }


        //  CREATE


        [HttpPost]
        public async Task<IActionResult> CreateMovie(CreateMovieDTO movieDTO, IFormFile? posterFile)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (posterFile != null && posterFile.Length > 0)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "movies");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(posterFile.FileName);
                var fullPath = Path.Combine(folder, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await posterFile.CopyToAsync(stream);
                movieDTO.PosterImg = "/images/movies/" + fileName;
            }

            var result = await _movieService.GetCreateMovie(movieDTO);

            if (!result.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, result.mensaje ?? "Could not create the movie.");
                return BadRequest(result);
            }

            return Json(result);
        }


        //  EDIT


        [HttpGet]
        public async Task<IActionResult> EditMovie(int id)
        {
            var result = await _movieService.GetMovieById(id);

            if (!result.EsCorrecto)
            {
                TempData["Error"] = result.mensaje ?? "Movie not found.";
                return RedirectToAction(nameof(Movies));
            }

            return View("~/Views/Movie/EditMovie.cshtml", result.Dato);
        }


        [HttpPost]
        public async Task<IActionResult> EditMovie(int id, CreateMovieDTO movieDTO, IFormFile? posterFile)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (posterFile != null && posterFile.Length > 0)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "movies");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(posterFile.FileName);
                var fullPath = Path.Combine(folder, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await posterFile.CopyToAsync(stream);
                movieDTO.PosterImg = "/images/movies/" + fileName;
            }

            var result = await _movieService.GetUpdateMovie(id, movieDTO);

            if (!result.EsCorrecto)
                return BadRequest(result);

            return Json(result);
        }


        //  DELETE


        [HttpPost]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var result = await _movieService.GetDeleteMovie(id);

            if (!result.EsCorrecto)
                return NotFound(result);

            return Json(result);
        }
    }
}
