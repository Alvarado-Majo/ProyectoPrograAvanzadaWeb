using System.ComponentModel.DataAnnotations;

namespace CineStreamCR.BLL.DTO.WatchList
{
    public class CreateWatchListDTO
    {
        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = null!;

        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string? Description { get; set; }
    }
}