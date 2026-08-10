using CineStreamCR.BLL.DTO.WatchList;
using CineStreamCR.BLL.Services.WatchList;
using Microsoft.AspNetCore.Mvc;

namespace StreamingApp.Controllers
{
    public class WatchListController : Controller
    {
        private readonly IWatchListService _watchListService;
        private readonly IWatchListMovieService _watchListMovieService;

        public WatchListController(
            IWatchListService watchListService,
            IWatchListMovieService watchListMovieService)
        {
            _watchListService = watchListService;
            _watchListMovieService = watchListMovieService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var watchListsResult =
                await _watchListService.GetWatchListsByUserId(userId.Value);

            var watchList =
                watchListsResult.Dato?.FirstOrDefault();

            // If the user does not have a list yet, create one
            if (watchList == null)
            {
                var createResult =
                    await _watchListService.CreateWatchList(
                        new CreateWatchListDTO
                        {
                            UserId = userId.Value,
                            Name = "My List",
                            Description = "My favorite movies"
                        });

                if (!createResult.EsCorrecto || createResult.Dato == null)
                {
                    TempData["Error"] =
                        createResult.mensaje ??
                        "Could not create My List.";

                    return View(
                        "MyList",
                        new WatchListDetailDTO
                        {
                            UserId = userId.Value,
                            Name = "My List"
                        });
                }

                watchList = createResult.Dato;
            }

            var moviesResult =
                await _watchListMovieService
                    .GetByWatchListId(watchList.WatchListId);

            var viewModel =
                new WatchListDetailDTO
                {
                    WatchListId = watchList.WatchListId,
                    UserId = watchList.UserId,
                    Name = watchList.Name,
                    Description = watchList.Description,
                    CreatedAt = watchList.CreatedAt,
                    Movies = moviesResult.Dato?
                        .ToList()
                        ?? new List<WatchListMovieDTO>()
                };

            return View("MyList", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMovie(int movieId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var watchListsResult =
                await _watchListService.GetWatchListsByUserId(userId.Value);

            var watchList =
                watchListsResult.Dato?.FirstOrDefault();

            if (watchList == null)
            {
                var createResult =
                    await _watchListService.CreateWatchList(
                        new CreateWatchListDTO
                        {
                            UserId = userId.Value,
                            Name = "My List",
                            Description = "My favorite movies"
                        });

                if (!createResult.EsCorrecto ||
                    createResult.Dato == null)
                {
                    TempData["Error"] =
                        createResult.mensaje ??
                        "Could not create My List.";

                    return RedirectToAction(
                        "Details",
                        "Movie",
                        new { id = movieId });
                }

                watchList = createResult.Dato;
            }

            var result =
                await _watchListMovieService.AddMovieToWatchList(
                    new WatchListMovieDTO
                    {
                        WatchListId = watchList.WatchListId,
                        MovieId = movieId
                    });

            if (result.EsCorrecto)
            {
                TempData["Success"] =
                    "Movie added to My List.";
            }
            else
            {
                TempData["Error"] =
                    result.mensaje ??
                    "Could not add the movie.";
            }

            return RedirectToAction(
                "Details",
                "Movie",
                new { id = movieId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMovie(int movieId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var watchListsResult =
                await _watchListService.GetWatchListsByUserId(userId.Value);

            var watchList =
                watchListsResult.Dato?.FirstOrDefault();

            if (watchList == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var result =
                await _watchListMovieService
                    .RemoveMovieFromWatchList(
                        watchList.WatchListId,
                        movieId);

            if (result.EsCorrecto)
            {
                TempData["Success"] =
                    "Movie removed from My List.";
            }
            else
            {
                TempData["Error"] =
                    result.mensaje ??
                    "Could not remove the movie.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}