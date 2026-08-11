using CineStreamCR.BLL.DTO.Review;
using CineStreamCR.BLL.Services.Review;
using Microsoft.AspNetCore.Mvc;

namespace CineStreamCR.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // Resumen: total de reviews, likes, dislikes y MovieRating (0-10)
        [HttpGet]
        public async Task<IActionResult> Summary(int movieId)
        {
            var summaryResult =
                await _reviewService.GetReviewSummaryByMovieIdAsync(movieId);

            if (!summaryResult.EsCorrecto)
            {
                return Json(new { esCorrecto = false, mensaje = summaryResult.mensaje });
            }

            // Si el usuario está logueado, se indica si ya votó y qué votó
            int? userId = HttpContext.Session.GetInt32("UserId");
            bool? userVoteIsLike = null;

            if (userId.HasValue)
            {
                var userReviewsResult =
                    await _reviewService.GetReviewsByMovieIdAsync(movieId);

                var userReview = userReviewsResult.Dato?
                    .FirstOrDefault(r => r != null && r.UserId == userId.Value);

                if (userReview != null)
                {
                    userVoteIsLike = userReview.IsLike;
                }
            }

            return Json(new
            {
                esCorrecto = true,
                dato = summaryResult.Dato,
                userLoggedIn = userId.HasValue,
                userVoteIsLike
            });
        }

        // Lista de reviews (con comentario) de una película, para mostrarlas debajo
        [HttpGet]
        public async Task<IActionResult> ByMovie(int movieId)
        {
            var reviewsResult =
                await _reviewService.GetReviewsByMovieIdAsync(movieId);

            return Json(new
            {
                esCorrecto = reviewsResult.EsCorrecto,
                dato = reviewsResult.Dato
            });
        }

        // Vota manita arriba/abajo. Si el usuario ya había votado, actualiza su voto.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Vote(int movieId, bool isLike, string? comment)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return Json(new
                {
                    esCorrecto = false,
                    mensaje = "You must be logged in to review a movie.",
                    requiresLogin = true
                });
            }

            var existingReviewsResult =
                await _reviewService.GetReviewsByMovieIdAsync(movieId);

            var existingReview = existingReviewsResult.Dato?
                .FirstOrDefault(r => r != null && r.UserId == userId.Value);

            if (existingReview != null)
            {
                var updateDto = new CreateReviewDTO
                {
                    UserId = userId.Value,
                    MovieId = movieId,
                    IsLike = isLike,
                    Comment = comment ?? existingReview.Comment
                };

                var updateResult =
                    await _reviewService.GetUpdateReviewAsync(existingReview.ReviewId, updateDto);

                return Json(new
                {
                    esCorrecto = updateResult.EsCorrecto,
                    mensaje = updateResult.mensaje
                });
            }

            var createDto = new CreateReviewDTO
            {
                UserId = userId.Value,
                MovieId = movieId,
                IsLike = isLike,
                Comment = comment
            };

            var createResult =
                await _reviewService.GetCreateReviewAsync(createDto);

            return Json(new
            {
                esCorrecto = createResult.EsCorrecto,
                mensaje = createResult.mensaje
            });
        }

        // Quita el voto/reseña del usuario actual para esa película
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveVote(int movieId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return Json(new { esCorrecto = false, mensaje = "You must be logged in." });
            }

            var existingReviewsResult =
                await _reviewService.GetReviewsByMovieIdAsync(movieId);

            var existingReview = existingReviewsResult.Dato?
                .FirstOrDefault(r => r != null && r.UserId == userId.Value);

            if (existingReview == null)
            {
                return Json(new { esCorrecto = false, mensaje = "You have not reviewed this movie." });
            }

            var deleteResult =
                await _reviewService.GetDeleteReviewAsync(existingReview.ReviewId);

            return Json(new
            {
                esCorrecto = deleteResult.EsCorrecto,
                mensaje = deleteResult.mensaje
            });
        }
    }
}
