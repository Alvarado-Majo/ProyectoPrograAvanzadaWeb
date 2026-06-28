using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CineStreamCR.DAL.Entities
{
    [Table("MovieActors")]
    public class MovieActors
    {
        public int? MovieId { get; set; }
        public int? ActorId { get; set; }
        public string CharacterName { get; set; } = string.Empty;

        //public virtual Movies Movie { get; set; } = null!;
        public virtual Actors Actors { get; set; } = null!;
    }
}
