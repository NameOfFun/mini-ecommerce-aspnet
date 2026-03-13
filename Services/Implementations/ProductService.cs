using Microsoft.EntityFrameworkCore;
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

        public async Task<Product> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
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

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.Include(c => c.Category).ToListAsync();
        }

        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Products.Include(c => c.Category).FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<bool> UpdateProduct(int id, Product product)
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
