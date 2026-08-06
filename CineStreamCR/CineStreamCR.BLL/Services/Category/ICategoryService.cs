using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Category;

namespace CineStreamCR.BLL.Services.Category
{
    public interface ICategoryService
    {
        Task<Answer<List<CategoryDTO?>>> GetAllCategoriesAsync();
        Task<Answer<CategoryDTO?>> GetCategoryByIdAsync(int id);
        Task<Answer<CategoryDTO>> GetCreateCategoryAsync(CreateCategoryDTO categoryDTO);
        Task<Answer<CategoryDTO>> GetUpdateCategoryAsync(int id, CreateCategoryDTO categoryDTO);
        Task<Answer<CategoryDTO>> GetDeleteCategoryAsync(int id);

        // Consultas adicionales
        Task<Answer<List<CategoryDTO?>>> GetCategoriesByMovieIdAsync(int movieId);
    }
}