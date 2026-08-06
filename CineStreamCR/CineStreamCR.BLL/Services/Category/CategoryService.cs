using AutoMapper;
using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Category;
using CineStreamCR.DAL.Repositories.Categories;

namespace CineStreamCR.BLL.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMovieCategoryRepository _movieCategoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMovieCategoryRepository movieCategoryRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _movieCategoryRepository = movieCategoryRepository;
            _mapper = mapper;
        }

        public async Task<Answer<List<CategoryDTO?>>> GetAllCategoriesAsync()
        {
            var answer = new Answer<List<CategoryDTO?>>();
            var categories = await _categoryRepository.GetCategories();
            answer.Dato = _mapper.Map<List<CategoryDTO?>>(categories);
            answer.EsCorrecto = true;
            return answer;
        }

        public async Task<Answer<CategoryDTO?>> GetCategoryByIdAsync(int id)
        {
            var answer = new Answer<CategoryDTO?>();
            var category = await _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Category not found.";
                answer.codigo = 404;
                return answer;
            }
            answer.EsCorrecto = true;
            answer.Dato = _mapper.Map<CategoryDTO?>(category);
            return answer;
        }

        public async Task<Answer<CategoryDTO>> GetCreateCategoryAsync(CreateCategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
            {
                return new Answer<CategoryDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Invalid category.",
                    codigo = 400
                };
            }

            var existing = await _categoryRepository.GetCategoryByName(categoryDTO.Name);
            if (existing != null)
            {
                return new Answer<CategoryDTO>
                {
                    EsCorrecto = false,
                    mensaje = "A category with that name already exists.",
                    codigo = 400
                };
            }

            var newCategory = _mapper.Map<DAL.Entities.Categories>(categoryDTO);

            bool result = await _categoryRepository.CreateCategory(newCategory);
            if (result)
            {
                return new Answer<CategoryDTO>
                {
                    EsCorrecto = true,
                    mensaje = "Category created successfully.",
                    Dato = _mapper.Map<CategoryDTO>(newCategory),
                    codigo = 201
                };
            }

            return new Answer<CategoryDTO>
            {
                EsCorrecto = false,
                mensaje = "Error creating the category.",
                codigo = 500
            };
        }

        public async Task<Answer<CategoryDTO>> GetUpdateCategoryAsync(int id, CreateCategoryDTO categoryDTO)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return new Answer<CategoryDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Category not found.",
                    codigo = 404
                };
            }

            var existingWithName = await _categoryRepository.GetCategoryByName(categoryDTO.Name);
            if (existingWithName != null && existingWithName.CategoryId != id)
            {
                return new Answer<CategoryDTO>
                {
                    EsCorrecto = false,
                    mensaje = "A category with that name already exists.",
                    codigo = 400
                };
            }

            category.Name = categoryDTO.Name;

            bool result = await _categoryRepository.UpdateCategory(category);
            if (result)
            {
                return new Answer<CategoryDTO>
                {
                    EsCorrecto = true,
                    mensaje = "Category updated successfully.",
                    Dato = _mapper.Map<CategoryDTO>(category),
                    codigo = 200
                };
            }

            return new Answer<CategoryDTO>
            {
                EsCorrecto = false,
                mensaje = "Error updating the category.",
                codigo = 500
            };
        }

        public async Task<Answer<CategoryDTO>> GetDeleteCategoryAsync(int id)
        {
            var answer = new Answer<CategoryDTO>();
            bool result = await _categoryRepository.DeleteCategory(id);
            if (result)
            {
                answer.EsCorrecto = true;
                answer.mensaje = "Category deleted successfully.";
                answer.codigo = 200;
                return answer;
            }

            answer.EsCorrecto = false;
            answer.mensaje = "Error deleting the category.";
            answer.codigo = 500;
            return answer;
        }

        public async Task<Answer<List<CategoryDTO?>>> GetCategoriesByMovieIdAsync(int movieId)
        {
            var answer = new Answer<List<CategoryDTO?>>();

            var movieCategories = await _movieCategoryRepository.GetByMovieId(movieId);

            var categories = movieCategories
                .Select(mc => mc.Category)
                .Where(c => c != null)
                .ToList();

            answer.Dato = _mapper.Map<List<CategoryDTO?>>(categories);
            answer.EsCorrecto = true;
            return answer;
        }
    }
}