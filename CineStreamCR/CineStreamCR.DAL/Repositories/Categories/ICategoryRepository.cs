using CineStreamCR.DAL.Entities;

namespace CineStreamCR.DAL.Repositories.Categories
{
    public interface ICategoryRepository
    {
        // CRUD
        Task<List<Entities.Categories>> GetCategories();
        Task<Entities.Categories?> GetCategoryById(int id);
        Task<Entities.Categories?> GetCategoryByName(string name);
        Task<bool> CreateCategory(Entities.Categories category);
        Task<bool> UpdateCategory(Entities.Categories category);
        Task<bool> DeleteCategory(int id);
    }
}