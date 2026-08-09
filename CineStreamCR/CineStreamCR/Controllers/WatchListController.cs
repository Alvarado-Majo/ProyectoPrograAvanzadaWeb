using CineStreamCR.BLL.DTO.WatchList;
using CineStreamCR.BLL.Services.WatchList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineStreamCR.Controllers
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


        //  VIEWS


        [HttpGet]
        public IActionResult MyWatchList()
        {
            return View();
        }


        //  READ (JSON) - WatchList


        [HttpGet]
        public async Task<IActionResult> GetWatchListById(int id)
        {
            var result = await _watchListService.GetWatchListById(id);
            if (!result.EsCorrecto)
                return NotFound(result);

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetWatchListsByUser(int userId)
        {
            var result = await _watchListService.GetWatchListsByUserId(userId);
            return Json(result);
        }


        //  CREATE - WatchList


        [HttpPost]
        public async Task<IActionResult> CreateWatchList(CreateWatchListDTO watchListDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _watchListService.CreateWatchList(watchListDTO);

            if (!result.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, result.mensaje ?? "Could not create the watchlist.");
                return BadRequest(result);
            }

            return Json(result);
        }


        //  EDIT - WatchList


        [HttpPost]
        public async Task<IActionResult> EditWatchList(WatchListDTO watchListDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _watchListService.UpdateWatchList(watchListDTO);

            if (!result.EsCorrecto)
                return BadRequest(result);

            return Json(result);
        }


        //  DELETE - WatchList


        [HttpPost]
        public async Task<IActionResult> DeleteWatchList(int id)
        {
            var result = await _watchListService.DeleteWatchList(id);

            if (!result.EsCorrecto)
                return BadRequest(result);

            return Json(result);
        }


        //  MOVIES DENTRO DE LA WATCHLIST


        [HttpGet]
        public async Task<IActionResult> GetMoviesInWatchList(int watchListId)
        {
            var result = await _watchListMovieService.GetByWatchListId(watchListId);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetWatchListsContainingMovie(int movieId)
        {
            var result = await _watchListMovieService.GetByMovieId(movieId);
            return Json(result);
        }

       
        [HttpPost]
        public async Task<IActionResult> AddMovieToWatchList(int watchListId, int movieId)
        {
            var dto = new WatchListMovieDTO
            {
                WatchListId = watchListId,
                MovieId = movieId
            };

            var result = await _watchListMovieService.AddMovieToWatchList(dto);

            if (!result.EsCorrecto)
                return BadRequest(result);

            return Json(result);
        }

        // Pasa el ID del usuario de una vez, no se ocupa poner en la vista
        [HttpPost]
        public async Task<IActionResult> AddMovieToMyWatchList(int userId, int movieId)
        {
            var watchLists = await _watchListService.GetWatchListsByUserId(userId);
            var watchList = watchLists.Dato?.FirstOrDefault();

            if (watchList == null)
            {
                return NotFound(new { EsCorrecto = false, mensaje = "El usuario no tiene una lista de deseos." });
            }

            var dto = new WatchListMovieDTO
            {
                WatchListId = watchList.WatchListId,
                MovieId = movieId
            };

            var result = await _watchListMovieService.AddMovieToWatchList(dto);

            if (!result.EsCorrecto)
                return BadRequest(result);

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveMovieFromWatchList(int watchListId, int movieId)
        {
            var result = await _watchListMovieService.RemoveMovieFromWatchList(watchListId, movieId);

            if (!result.EsCorrecto)
                return NotFound(result);

            return Json(result);
        }
    }
}
