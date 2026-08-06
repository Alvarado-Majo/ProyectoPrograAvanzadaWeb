using CineStreamCR.DAL.Data;
using CineStreamCR.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CineStreamCR.DAL.Repositories.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ProyectoDBContext _context;

        public CategoryRepository(ProyectoDBContext context)
        {
            _context = context;
        }

        public async Task<List<Entities.Categories>> GetCategories()
        {
            return await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Entities.Categories?> GetCategoryById(int id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<Entities.Categories?> GetCategoryByName(string name)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c =>
                    c.Name.ToLower() == name.ToLower().Trim());
        }

        public async Task<bool> CreateCategory(Entities.Categories category)
        {
            if (category == null)
                return false;

            await _context.Categories.AddAsync(category);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCategory(Entities.Categories category)
        {
            if (category == null)
                return false;

            var existingCategory =
                await _context.Categories.FindAsync(category.CategoryId);

            if (existingCategory == null)
                return false;

            existingCategory.Name = category.Name;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var entity = await _context.Categories.FindAsync(id);

            if (entity == null)
                return false;

            _context.Categories.Remove(entity);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}