using System.ComponentModel.DataAnnotations;

namespace CineStreamCR.BLL.DTO.Review
{
    public class CreateReviewDTO
    {
        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "MovieId is required.")]
        public int MovieId { get; set; }

        // "Sólo valores del 1 al 10"
        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10.")]
        public int Rating { get; set; }

        // Reseña escrita opcional.
        public string? Comment { get; set; }
    }
}