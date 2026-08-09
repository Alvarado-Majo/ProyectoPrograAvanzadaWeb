using CineStreamCR.BLL.DTO.Category;
using CineStreamCR.BLL.Services.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineStreamCR.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMovieCategoryService _movieCategoryService;

        public CategoryController(
            ICategoryService categoryService,
            IMovieCategoryService movieCategoryService)
        {
            _categoryService = categoryService;
            _movieCategoryService = movieCategoryService;
        }


        //  VIEWS

        [HttpGet]
        public IActionResult Categories()
        {
            return View();
        }


        //  READ (JSON) - Category

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _categoryService.GetAllCategoriesAsync();
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);
            if (!result.EsCorrecto)
                return NotFound(result);

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoriesByMovie(int movieId)
        {
            var result = await _categoryService.GetCategoriesByMovieIdAsync(movieId);
            return Json(result);
        }


        //  CREATE - Category

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _categoryService.GetCreateCategoryAsync(categoryDTO);

            if (!result.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, result.mensaje ?? "Could not create the category.");
                return BadRequest(result);
            }

            return Json(result);
        }


        //  EDIT - Category

        [HttpPost]
        public async Task<IActionResult> EditCategory(int id, CreateCategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _categoryService.GetUpdateCategoryAsync(id, categoryDTO);

            if (!result.EsCorrecto)
                return BadRequest(result);

            return Json(result);
        }


        //  DELETE - Category

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.GetDeleteCategoryAsync(id);

            if (!result.EsCorrecto)
                return NotFound(result);

            return Json(result);
        }



        //  ASIGNACIÓN A PELÍCULAS (endpoints de MovieCategories)


        [HttpGet]
        public async Task<IActionResult> GetMoviesByCategory(int categoryId)
        {
            var result = await _movieCategoryService.GetMoviesByCategoryId(categoryId);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> AssignCategoryToMovie(AssignCategoryToMovieDTO dto)
        {
            var result = await _movieCategoryService.AssignCategoryToMovie(dto);

            if (!result.EsCorrecto)
                return BadRequest(result);

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCategoryFromMovie(int movieId, int categoryId)
        {
            var result = await _movieCategoryService.RemoveCategoryFromMovie(movieId, categoryId);

            if (!result.EsCorrecto)
                return NotFound(result);

            return Json(result);
        }
    }
}
