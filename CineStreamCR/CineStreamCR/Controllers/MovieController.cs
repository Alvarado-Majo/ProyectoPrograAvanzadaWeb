using CineStreamCR.BLL.DTO.Movie;
using CineStreamCR.BLL.Services.Category;
using CineStreamCR.BLL.Services.Movie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StreamingApp.Models;

namespace CineStreamCR.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ICategoryService _categoryService;
        private readonly IMovieCategoryService _movieCategoryService;

        public MovieController(IMovieService movieService,ICategoryService categoryService,IMovieCategoryService movieCategoryService)
        {
            _movieService = movieService;
            _categoryService = categoryService;
            _movieCategoryService = movieCategoryService;
        }

        //  VIEWS


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Movies(
         int? categoryId = null,
         string? search = null)
        {
            var moviesResult = await _movieService.GetAllMovies();
            var categoriesResult = await _categoryService.GetAllCategoriesAsync();

            var movies = moviesResult.Dato?
                .Where(m => m != null)
                .ToList()
                ?? new List<MovieDTO>();

            // Filter by category
            if (categoryId.HasValue)
            {
                var movieCategoriesResult =
                    await _movieCategoryService.GetMoviesByCategoryId(categoryId.Value);

                if (movieCategoriesResult.EsCorrecto &&
                    movieCategoriesResult.Dato != null)
                {
                    var movieIds = movieCategoriesResult.Dato
                        .Select(mc => mc.MovieId)
                        .ToHashSet();

                    movies = movies
                        .Where(m => movieIds.Contains(m.MovieId))
                        .ToList();
                }
                else
                {
                    movies.Clear();
                }
            }

            // Search by title
            if (!string.IsNullOrWhiteSpace(search))
            {
                movies = movies
                    .Where(m =>
                        !string.IsNullOrWhiteSpace(m.Title) &&
                        m.Title.Contains(
                            search,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var viewModel = new MovieCatalogViewModel
            {
                Movies = movies,

                Categories = categoriesResult.Dato?
                    .Where(c => c != null)
                    .Select(c => c!)
                    .ToList()
                    ?? new(),

                SelectedCategoryId = categoryId,
                Search = search ?? string.Empty
            };

            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _movieService.GetMovieDetailsById(id);

            if (!result.EsCorrecto || result.Dato == null)
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
