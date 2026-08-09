using AutoMapper;
using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Category;
using CineStreamCR.DAL.Repositories.Categories;
using CineStreamCR.DAL.Repositories.Movies;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.Services.Category
{
    public class MovieCategoryService : IMovieCategoryService
    {
        private readonly IMovieCategoryRepository _movieCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public MovieCategoryService(
            IMovieCategoryRepository movieCategoryRepository,
            ICategoryRepository categoryRepository,
            IMovieRepository movieRepository,
            IMapper mapper)
        {
            _movieCategoryRepository = movieCategoryRepository;
            _categoryRepository = categoryRepository;
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<Answer<MovieCategoryDTO?>> AssignCategoryToMovie(AssignCategoryToMovieDTO dto)
        {
            var answer = new Answer<MovieCategoryDTO?>();

            var movie = await _movieRepository.GetMovieById(dto.MovieId);
            if (movie == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "La película no existe.";
                answer.codigo = 404;
                return answer;
            }

            var category = await _categoryRepository.GetCategoryById(dto.CategoryId);
            if (category == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "La categoría no existe.";
                answer.codigo = 404;
                return answer;
            }

            var existing = await _movieCategoryRepository.GetByMovieAndCategory(dto.MovieId, dto.CategoryId);
            if (existing != null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "La categoría ya está asignada a esta película.";
                answer.codigo = 400;
                return answer;
            }

            var newMovieCategory = new DAL.Entities.MovieCategories
            {
                MovieId = dto.MovieId,
                CategoryId = dto.CategoryId
            };

            bool result = await _movieCategoryRepository.AssignCategoryToMovie(newMovieCategory);
            if (!result)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Error al asignar la categoría a la película.";
                answer.codigo = 500;
                return answer;
            }

            answer.EsCorrecto = true;
            answer.mensaje = "Categoría asignada correctamente.";
            answer.codigo = 201;
            answer.Dato = new MovieCategoryDTO
            {
                MovieId = dto.MovieId,
                CategoryId = dto.CategoryId,
                CategoryName = category.Name,
                MovieTitle = movie.Title
            };
            return answer;
        }

        public async Task<Answer<bool>> RemoveCategoryFromMovie(int movieId, int categoryId)
        {
            var answer = new Answer<bool>();

            var existing = await _movieCategoryRepository.GetByMovieAndCategory(movieId, categoryId);
            if (existing == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "La categoría no está asignada a esta película.";
                answer.codigo = 404;
                return answer;
            }

            bool result = await _movieCategoryRepository.RemoveCategoryFromMovie(movieId, categoryId);
            if (!result)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Error al quitar la categoría de la película.";
                answer.codigo = 500;
                return answer;
            }

            answer.EsCorrecto = true;
            answer.mensaje = "Categoría removida correctamente.";
            answer.codigo = 200;
            answer.Dato = true;
            return answer;
        }

        public async Task<Answer<List<MovieCategoryDTO>>> GetCategoriesByMovieId(int movieId)
        {
            var answer = new Answer<List<MovieCategoryDTO>>();
            var movieCategories = await _movieCategoryRepository.GetByMovieId(movieId);
            answer.Dato = _mapper.Map<List<MovieCategoryDTO>>(movieCategories);
            answer.EsCorrecto = true;
            return answer;
        }

        public async Task<Answer<List<MovieCategoryDTO>>> GetMoviesByCategoryId(int categoryId)
        {
            var answer = new Answer<List<MovieCategoryDTO>>();
            var movieCategories = await _movieCategoryRepository.GetByCategoryId(categoryId);
            answer.Dato = _mapper.Map<List<MovieCategoryDTO>>(movieCategories);
            answer.EsCorrecto = true;
            return answer;
        }
    }
}
