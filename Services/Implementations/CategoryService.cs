using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Mini_E_Commerce.Dtos.Category;
using Mini_E_Commerce.Models;
using Mini_E_Commerce.Services.Interface;

namespace Mini_E_Commerce.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly EcommerceMiniContext _context;
        public CategoryService(EcommerceMiniContext context)
        {
            _context = context;
        }
        public async Task<CategoryDto> CreateCategory(CategoryDto categoryDto)
        {
            var category = new Category
            {
                CategoryName = categoryDto.CategoryName,
                CategoryAlias = categoryDto.CategoryAlias,
                Description = categoryDto.Description,
                Image = categoryDto.Image
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            categoryDto.CategoryId = category.CategoryId;
            return categoryDto;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == id);

            if (hasProducts)
            {
                return false;
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            return await _context.Categories.AsNoTracking().Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                CategoryAlias = c.CategoryAlias,
                Description = c.Description,
                Image = c.Image
            }).ToListAsync();
        }

        public async Task<CategoryDto?> GetCategoryById(int id)
        {
            return await _context.Categories.AsNoTracking().Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                CategoryAlias = c.CategoryAlias,
                Description = c.Description,
                Image = c.Image
            }).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateCategory(int id, CategoryDto categoryDto)
        {
            var categoryExisting = await _context.Categories.FindAsync(id);

            if (categoryExisting == null)
            {
                return false;
            }

            categoryExisting.CategoryName = categoryDto.CategoryName;
            categoryExisting.CategoryAlias = categoryDto.CategoryAlias;
            categoryExisting.Description = categoryDto.Description;
            categoryExisting.Image = categoryDto.Image;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
