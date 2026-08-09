using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.DTO.Director
{
    public class MovieDirectorDTO
    {
        public int MovieId { get; set; }
        public int DirectorId { get; set; }

        public string? DirectorFullName { get; set; }
        public string? DirectorPictureImg { get; set; }

        public string? MovieTitle { get; set; }
        public string? MoviePosterImg { get; set; }
    }
    //par asigar director a pelicula
    public class AssignDirectorToMovieDTO
    {
        public int MovieId { get; set; }
        public int DirectorId { get; set; }
    }
}

