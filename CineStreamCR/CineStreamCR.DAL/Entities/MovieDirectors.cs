using System.ComponentModel.DataAnnotations.Schema;

namespace CineStreamCR.DAL.Entities
{
    [Table("MovieDirectors")]
    public partial class MovieDirectors
    {
        public int MovieId { get; set; }
        public int DirectorId { get; set; }
        public virtual Movies Movie { get; set; } = null!;
        public virtual Directors Director { get; set; } = null!;
    }
}