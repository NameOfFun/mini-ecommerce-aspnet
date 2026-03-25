using Mini_E_Commerce.Dtos.Supplier;

namespace Mini_E_Commerce.Services.Interface
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDto>> GetAllSuppliers();
        Task<SupplierDto?> GetSupplierById(string id);
        Task<SupplierDto?> CreateSupplier(SupplierDto supplierDto);
        Task<bool> UpdateSupplier(string id, SupplierDto supplierDto);
        Task<bool> DeleteSupplier(string id);
    }
}
