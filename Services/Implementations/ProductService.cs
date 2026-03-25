using Microsoft.EntityFrameworkCore;
using Mini_E_Commerce.Dtos.Product;
using Mini_E_Commerce.Models;
using Mini_E_Commerce.Services.Interface;

namespace Mini_E_Commerce.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly EcommerceMiniContext _context;
        public ProductService(EcommerceMiniContext context)
        {
            _context = context;
        }

        public async Task<ProductDto?> CreateProduct(ProductDto productDto)
        {
            var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == productDto.CategoryId);

            if (!categoryExists)
            {
                throw new Exception("Category not found");
            } 

            var product = new Product
            {
                ProductName = productDto.ProductName,
                ProductAlias = productDto.ProductAlias,
                CategoryId = productDto.CategoryId,
                UnitDescription = productDto.UnitDescription,
                UnitPrice = productDto.UnitPrice,
                Image = productDto.Image,
                ManufactureDate = productDto.ManufactureDate,
                Discount = productDto.Discount,
                ViewCount = productDto.ViewCount,
                Description = productDto.Description,
                SupplierId = productDto.SupplierId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            productDto.ProductId = product.ProductId;
            return productDto;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return false;
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            return await _context.Products.AsNoTracking().Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductAlias = p.ProductAlias,
                CategoryId = p.CategoryId,
                UnitDescription = p.UnitDescription,
                UnitPrice = p.UnitPrice,
                Image = p.Image,
                ManufactureDate = p.ManufactureDate,
                Discount = p.Discount,
                ViewCount = p.ViewCount,
                Description = p.Description,
                SupplierId = p.SupplierId,
                CategoryName = p.Category.CategoryName
            }).ToListAsync();
        }

        public async Task<ProductDto?> GetProductById(int id)
        {
            return await _context.Products.AsNoTracking().Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductAlias = p.ProductAlias,
                CategoryId = p.CategoryId,
                UnitDescription = p.UnitDescription,
                UnitPrice = p.UnitPrice,
                Image = p.Image,
                ManufactureDate = p.ManufactureDate,
                Discount = p.Discount,
                ViewCount = p.ViewCount,
                Description = p.Description,
                SupplierId = p.SupplierId,
                CategoryName = p.Category.CategoryName
            }).FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<bool> UpdateProduct(int id, ProductDto product)
        {
            var existingProduct = await _context.Products.FindAsync(id);

            if (existingProduct == null)
            {
                return false;
            }

            existingProduct.ProductName = product.ProductName;
            existingProduct.UnitPrice = product.UnitPrice;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.Description = product.Description;
            existingProduct.UnitDescription = product.UnitDescription;
            existingProduct.Image = product.Image;
            existingProduct.SupplierId = product.SupplierId;
            existingProduct.ManufactureDate = product.ManufactureDate;
            existingProduct.Discount = product.Discount;
            existingProduct.ViewCount = product.ViewCount;
            existingProduct.ProductAlias = product.ProductAlias;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
