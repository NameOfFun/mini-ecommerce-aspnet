using Microsoft.EntityFrameworkCore;
using Mini_E_Commerce.Dtos.Common;
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

        public async Task<PagedResultDto<ProductDto>> GetAllProducts(ProductQueryDto query)
        {
            // start tu  IQueryabl - chua thuc thi truy van
            var queryable = _context.Products.AsNoTracking().AsQueryable();

            // Search by name
            if (!string.IsNullOrEmpty(query.Search))
            {
                var keyword = query.Search.Trim().ToLower();
                queryable = queryable.Where(p => p.ProductName.ToLower().Contains(keyword) || (p.Description != null && p.Description.ToLower().Contains(keyword)));
            }
            // Filter by category
            if(query.CategoryId.HasValue)
            {
                queryable = queryable.Where(p => p.CategoryId == query.CategoryId.Value);
            }

            // Sort
            queryable = query.SortBy?.ToLower() switch
            {
                "price" => query.IsDescending
                        ? queryable.OrderByDescending(p => p.UnitPrice)
                        : queryable.OrderBy(p => p.UnitPrice),
                "date" => query.IsDescending
                                ? queryable.OrderByDescending(p => p.ManufactureDate)
                                : queryable.OrderBy(p => p.ManufactureDate),
                _ => query.IsDescending                           // mặc định sort theo tên
                                ? queryable.OrderByDescending(p => p.ProductName)
                                : queryable.OrderBy(p => p.ProductName),
            };

            // dem tong (truoc khi phan trang)
            var totalCount = await queryable.CountAsync();

            // Phan trang -- Pagination
            var items = await queryable
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(p => new ProductDto
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

            return new PagedResultDto<ProductDto>
            {
                Data = items,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount
            };
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
