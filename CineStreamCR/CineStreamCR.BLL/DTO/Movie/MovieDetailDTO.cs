using System;
using System.Collections.Generic;

namespace CineStreamCR.BLL.DTO.Movie
{
    // Para la vista "Detalle de Película" que incluye: sinopsis, duración, año, géneros, rating, director(es), elenco.
    public class MovieDetailDTO
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Synopsis { get; set; } = string.Empty;
        public decimal? MovieRating { get; set; }
        public int ReleaseYear { get; set; }
        public int DurationMinutes { get; set; }
        public string PosterImg { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public byte IsActive { get; set; } = 1;

        public List<MovieDirectorSummaryDTO> Directors { get; set; } = new();
        public List<MovieCastMemberDTO> Cast { get; set; } = new();
        public List<MovieCategorySummaryDTO> Categories { get; set; } = new();
    }
}