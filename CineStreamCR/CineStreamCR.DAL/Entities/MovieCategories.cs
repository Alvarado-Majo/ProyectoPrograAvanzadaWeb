using System.ComponentModel.DataAnnotations.Schema;

namespace CineStreamCR.DAL.Entities
{
    [Table("MovieCategories")]
    public partial class MovieCategories
    {
        public int MovieId { get; set; }
        public int CategoryId { get; set; }
        public virtual Movies Movie { get; set; } = null!;
        public virtual Categories Category { get; set; } = null!;
    }
}