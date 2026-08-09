using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.Services.Category
{
    public interface IMovieCategoryService
    {
        Task<Answer<MovieCategoryDTO?>> AssignCategoryToMovie(AssignCategoryToMovieDTO dto);
        Task<Answer<bool>> RemoveCategoryFromMovie(int movieId, int categoryId);

        // Consultas
        Task<Answer<List<MovieCategoryDTO>>> GetCategoriesByMovieId(int movieId);
        Task<Answer<List<MovieCategoryDTO>>> GetMoviesByCategoryId(int categoryId);
    }
}
