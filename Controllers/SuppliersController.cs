using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_E_Commerce.Dtos.Supplier;
using Mini_E_Commerce.Services.Interface;

namespace Mini_E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliers();
            return Ok(suppliers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById(string id)
        {
            var supplier = await _supplierService.GetSupplierById(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return Ok(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] SupplierDto supplierDto)
        {
            var createdSupplier = await _supplierService.CreateSupplier(supplierDto);
            if (createdSupplier == null)
            {
                return BadRequest("Failed to create supplier");
            }
            return CreatedAtAction(nameof(GetSupplierById), new { id = createdSupplier.SupplierId }, createdSupplier);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(string id, [FromBody] SupplierDto supplier)
        {
            var updatedSupplier = await _supplierService.UpdateSupplier(id, supplier);
            if (!updatedSupplier)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(string id)
        {
            var result = await _supplierService.DeleteSupplier(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
