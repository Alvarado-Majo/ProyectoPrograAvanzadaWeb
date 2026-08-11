using System.ComponentModel.DataAnnotations;

namespace CineStreamCR.BLL.DTO.Review
{
    public class CreateReviewDTO
    {
        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "MovieId is required.")]
        public int MovieId { get; set; }

        // Manita arriba (true) o manita abajo (false)
        [Required(ErrorMessage = "IsLike is required.")]
        public bool IsLike { get; set; }

        // Reseña escrita opcional.
        public string? Comment { get; set; }
    }
}