
namespace CineStreamCR.DAL.Entities
{
    public partial class Categories
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<MovieCategories> MovieCategories { get; set; }
            = new List<MovieCategories>();
    }
}
