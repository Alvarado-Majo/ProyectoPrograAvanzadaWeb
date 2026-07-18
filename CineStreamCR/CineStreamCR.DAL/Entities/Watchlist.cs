using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineStreamCR.DAL.Entities
{
    [Table("WatchLists")]
    public partial class WatchLists
    {
        public int WatchListId { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public virtual Users User { get; set; } = null!;

        public virtual ICollection<WatchListMovies> WatchListMovies { get; set; }
            = new List<WatchListMovies>();
    }
}