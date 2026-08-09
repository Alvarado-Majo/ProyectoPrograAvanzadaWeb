using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.DTO.Actor
{
    public class MovieActorDTO
    {
        public int MovieId { get; set; }
        public int ActorId { get; set; }
        public string CharacterName { get; set; } = string.Empty;

        public string? ActorFullName { get; set; }
        public string? ActorPictureImg { get; set; }

        public string? MovieTitle { get; set; }
        public string? MoviePosterImg { get; set; }
    }
    //par asignar un actor a una pelicula
    public class AssignActorToMovieDTO
    {
        public int MovieId { get; set; }
        public int ActorId { get; set; }
        public string CharacterName { get; set; } = string.Empty;
    }
}

