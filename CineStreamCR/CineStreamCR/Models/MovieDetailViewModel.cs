using CineStreamCR.BLL.DTO.Movie;

namespace StreamingApp.Models
{
    public class MovieDetailViewModel
    {
        public MovieDetailDTO Movie { get; set; } = new();

        public int? PreviousMovieId { get; set; }

        public int? NextMovieId { get; set; }
    }
}
