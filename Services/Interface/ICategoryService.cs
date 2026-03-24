using Mini_E_Commerce.Dtos.Category;

namespace Mini_E_Commerce.Services.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategories();
        Task<CategoryDto?> GetCategoryById(int id);
        Task<CategoryDto> CreateCategory(CategoryDto categoryDto);
        Task<bool> UpdateCategory(int id, CategoryDto categoryDto);
        Task<bool> DeleteCategory(int id);
    }
}
