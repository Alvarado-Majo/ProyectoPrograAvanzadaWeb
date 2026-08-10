using CineStreamCR.BLL.Services.Category;
using CineStreamCR.BLL.Services.Movie;
using CineStreamCR.BLL.Services.WatchList;
using Microsoft.AspNetCore.Mvc;
using StreamingApp.Models;

namespace StreamingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ICategoryService _categoryService;
        private readonly IMovieCategoryService _movieCategoryService;
        private readonly IWatchListService _watchListService;
        private readonly IWatchListMovieService _watchListMovieService;

        public HomeController(
            IMovieService movieService,
            ICategoryService categoryService,
            IMovieCategoryService movieCategoryService,
            IWatchListService watchListService,
            IWatchListMovieService watchListMovieService)
        {
            _movieService = movieService;
            _categoryService = categoryService;
            _movieCategoryService = movieCategoryService;
            _watchListService = watchListService;
            _watchListMovieService = watchListMovieService;
        }

        public async Task<IActionResult> Index()
        {
            // Usuario que inició sesión
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            string userName =
                HttpContext.Session.GetString("UserName") ?? string.Empty;

            // Traer películas de la BD
            var moviesAnswer = await _movieService.GetAllMovies();

            var movies = moviesAnswer.Dato?
                .Where(x => x.IsActive == 1)
                .ToList() ?? new();

            // Traer categorías
            var categoriesAnswer =
                await _categoryService.GetAllCategoriesAsync();

            var categories =
                categoriesAnswer.Dato?.ToList() ?? new();

            // IDs de películas favoritas del usuario
            var favoriteMovieIds = new HashSet<int>();

            // Películas para "Mi Lista"
            var myList = new List<ContentItem>();

            // Obtener las listas del usuario
            var watchListsAnswer =
                await _watchListService.GetWatchListsByUserId(userId.Value);

            var userWatchList =
                watchListsAnswer.Dato?.FirstOrDefault();

            if (userWatchList != null)
            {
                var watchListMoviesAnswer =
                    await _watchListMovieService
                        .GetByWatchListId(userWatchList.WatchListId);

                if (watchListMoviesAnswer.Dato != null)
                {
                    foreach (var movie in watchListMoviesAnswer.Dato)
                    {
                        favoriteMovieIds.Add(movie.MovieId);

                        myList.Add(new ContentItem
                        {
                            Id = movie.MovieId,
                            Title = movie.Title,
                            ThumbnailUrl = movie.PosterImg,
                            Year = movie.ReleaseYear,
                            DurationMinutes = movie.DurationMinutes,
                            Rating = movie.MovieRating,
                            IsInMyList = true
                        });
                    }
                }
            }

            // Crear las filas del catálogo por categoría
            var rows = new List<ContentRow>();

            foreach (var category in categories)
            {
                var movieCategoriesAnswer =
                    await _movieCategoryService
                        .GetMoviesByCategoryId(category.CategoryId);

                var movieIds =
                    movieCategoriesAnswer.Dato?
                    .Select(x => x.MovieId)
                    .ToHashSet()
                    ?? new HashSet<int>();

                var moviesInCategory =
                    movies
                    .Where(x => movieIds.Contains(x.MovieId))
                    .Select(x => new ContentItem
                    {
                        Id = x.MovieId,
                        Title = x.Title,
                        ThumbnailUrl = x.PosterImg,
                        Year = x.ReleaseYear,
                        DurationMinutes = x.DurationMinutes,
                        Rating = x.MovieRating,
                        IsInMyList =
                            favoriteMovieIds.Contains(x.MovieId)
                    })
                    .ToList();

                if (moviesInCategory.Any())
                {
                    rows.Add(new ContentRow
                    {
                        CategoryId = category.CategoryId,
                        Title = category.Name,
                        Items = moviesInCategory
                    });
                }
            }

            // Película destacada del inicio
            var featuredMovie = movies.FirstOrDefault();

            FeaturedContent? featuredContent = null;

            if (featuredMovie != null)
            {
                var categoriesMovie =
                    await _categoryService
                        .GetCategoriesByMovieIdAsync(
                            featuredMovie.MovieId
                        );

                string genres = string.Join(
                    " · ",
                    categoriesMovie.Dato?
                        .Select(x => x.Name)
                        ?? Enumerable.Empty<string>()
                );

                featuredContent = new FeaturedContent
                {
                    Id = featuredMovie.MovieId,
                    Title = featuredMovie.Title,
                    BackdropUrl = featuredMovie.PosterImg,
                    Description = string.Empty,
                    Year = featuredMovie.ReleaseYear,
                    DurationMinutes = featuredMovie.DurationMinutes,
                    Rating = featuredMovie.MovieRating,
                    Genre = genres,
                    IsInMyList = favoriteMovieIds.Contains(featuredMovie.MovieId),

                    ContentType = "PELÍCULA",
                    IsComingSoon = false,
                    ComingSoonLabel = string.Empty
                };
            }

            var vm = new HomeViewModel
            {
                FeaturedContent = featuredContent,
                Rows = rows,
                MyList = myList,
                UserName = userName
            };

            return View(vm);
        }

        public IActionResult Detalles(int id)
        {
            return RedirectToAction(
                "Details",
                "Movie",
                new { id = id }
            );
        }

        public IActionResult MyList()
        {
            return View();
        }
    }
}