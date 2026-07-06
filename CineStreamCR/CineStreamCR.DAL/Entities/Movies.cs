using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineStreamCR.DAL.Entities
{
    [Table("Movies")]
    public class Movies
    {
        public int MovieId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Synopsis { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public int DurationMinutes { get; set; }
        public decimal Rating { get; set; }

        public string PosterImg { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;

        public byte IsActive { get; set; } = 1;

        public int DirectorId { get; set; }
        public virtual Directors Director { get; set; } = null!;

        public virtual ICollection<MovieActors> MovieActors { get; set; } = new List<MovieActors>();
    }
}