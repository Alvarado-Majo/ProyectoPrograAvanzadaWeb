using System.ComponentModel.DataAnnotations.Schema;

namespace CineStreamCR.DAL.Entities
{
    [Table("WatchListMovies")]
    public partial class WatchListMovies
    {
        public int WatchListId { get; set; }
        public int MovieId { get; set; }
        public virtual WatchLists WatchList { get; set; } = null!;
        public virtual Movies Movie { get; set; } = null!;
    }
}