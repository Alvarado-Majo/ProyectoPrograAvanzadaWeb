using System.ComponentModel.DataAnnotations;

namespace CineStreamCR.BLL.DTO.Category
{
    //Admin use si se definen roles y permisos para crear categorias.
    public class CreateCategoryDTO
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = null!;
    }
}