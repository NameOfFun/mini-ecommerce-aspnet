using Microsoft.EntityFrameworkCore;
using Mini_E_Commerce.Dtos.Supplier;
using Mini_E_Commerce.Models;
using Mini_E_Commerce.Services.Interface;

namespace Mini_E_Commerce.Services.Implementations
{
    public class SupplierService : ISupplierService
    {
        private readonly EcommerceMiniContext _context;
        public SupplierService(EcommerceMiniContext context)
        {
            _context = context;
        }
        public async Task<SupplierDto?> CreateSupplier(SupplierDto supplierDto)
        {
            var supplier = new Supplier
            {
                SupplierId = supplierDto.SupplierId,
                CompanyName = supplierDto.CompanyName,
                Logo = supplierDto.Logo,
                ContactPerson = supplierDto.ContactPerson,
                Email = supplierDto.Email,
                Phone = supplierDto.Phone,
                Address = supplierDto.Address,
                Description = supplierDto.Description
            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return supplierDto;
        }

        public async Task<bool> DeleteSupplier(string id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return false;
            }
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SupplierDto>> GetAllSuppliers()
        {
            return await _context.Suppliers.AsNoTracking().Select(s => new SupplierDto
            {
                SupplierId = s.SupplierId,
                CompanyName = s.CompanyName,
                Logo = s.Logo,
                ContactPerson = s.ContactPerson,
                Email = s.Email,
                Phone = s.Phone,
                Address = s.Address,
                Description = s.Description
            }).ToListAsync();
        }

        public async Task<SupplierDto?> GetSupplierById(string id)
        {
            return await _context.Suppliers.AsNoTracking().Select(s => new SupplierDto
            {
                SupplierId = s.SupplierId,
                CompanyName = s.CompanyName,
                Logo = s.Logo,
                ContactPerson = s.ContactPerson,
                Email = s.Email,
                Phone = s.Phone,
                Address = s.Address,
                Description = s.Description
            }).FirstOrDefaultAsync(s => s.SupplierId == id);
        }

        public async Task<bool> UpdateSupplier(string id, SupplierDto supplierDto)
        {
            var supplierExisting = await _context.Suppliers.FindAsync(id);
            if (supplierExisting == null)
            {
                return false;
            }

            supplierExisting.CompanyName = supplierDto.CompanyName;
            supplierExisting.Logo = supplierDto.Logo;
            supplierExisting.ContactPerson = supplierDto.ContactPerson;
            supplierExisting.Email = supplierDto.Email;
            supplierExisting.Phone = supplierDto.Phone;
            supplierExisting.Address = supplierDto.Address;
            supplierExisting.Description = supplierDto.Description;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
