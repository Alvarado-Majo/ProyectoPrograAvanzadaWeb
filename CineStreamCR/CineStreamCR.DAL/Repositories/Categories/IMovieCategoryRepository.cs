using CineStreamCR.DAL.Entities;

namespace CineStreamCR.DAL.Repositories.Categories
{
    public interface IMovieCategoryRepository
    {

        Task<MovieCategories?> GetByMovieAndCategory(int movieId, int categoryId);

        Task<bool> AssignCategoryToMovie(MovieCategories movieCategory);

        Task<bool> RemoveCategoryFromMovie(int movieId,int categoryId);

        // Consultas

        Task<List<MovieCategories>> GetByMovieId(int movieId);

        Task<List<MovieCategories>> GetByCategoryId(int categoryId);
    }
}