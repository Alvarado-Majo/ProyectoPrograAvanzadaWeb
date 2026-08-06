using System.ComponentModel.DataAnnotations;

namespace CineStreamCR.BLL.DTO.Movie
{

    //Sólo para admin view en caso de que hagamos separación por roles
    public class CreateMovieDTO
    {
        [Key]
        public int MovieId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Synopsis is required.")]
        public string Synopsis { get; set; } = null!;

        [Required(ErrorMessage = "Release year is required.")]
        [Range(1888, 2100, ErrorMessage = "Enter a valid release year.")]
        public int ReleaseYear { get; set; }

        [Required(ErrorMessage = "Duration is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than 0.")]
        public int DurationMinutes { get; set; }

        public string? PosterImg { get; set; }

        public string? VideoUrl { get; set; }

        [Required(ErrorMessage = "Nationality is required.")]
        public string Nationality { get; set; } = string.Empty;

        [Required(ErrorMessage = "The status is required.")]
        public byte IsActive { get; set; } = 1;

        //Movie rating se calcula basado en un promedio de las calificaciones de los usuarios, por lo que no es necesario incluirlo en el DTO de creación.
    }
}