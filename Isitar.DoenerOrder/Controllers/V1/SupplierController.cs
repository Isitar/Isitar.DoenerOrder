using System.Collections.Generic;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Contracts.Requests;
using Isitar.DoenerOrder.Contracts.V1;
using Isitar.DoenerOrder.Data;
using Isitar.DoenerOrder.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Isitar.DoenerOrder.Controllers
{
    public class SupplierController : ApiController
    {
        private readonly DoenerOrderContext dbContext;
        private readonly SupplierService supplierService;

        public SupplierController(DoenerOrderContext dbContext, SupplierService supplierService)
        {
            this.dbContext = dbContext;
            this.supplierService = supplierService;
        }
        
        [HttpGet(ApiRoutes.Suppliers.GetAll)]
        public async Task<ActionResult<IAsyncEnumerable<SupplierDTO>>> GetAll()
        {
            return Ok(await supplierService.GetAllAsync());
        }
        [HttpGet(ApiRoutes.Suppliers.Get)]
        public async Task<ActionResult<SupplierDTO>> Get(int supplierId)
        {
            var supplier = await supplierService.GetAsync(supplierId);
            if (null == supplier)
            {
                return NotFound();
            }

            return Ok(supplier);
        }


        /// <summary>
        /// Creates a supplier
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns>the updated supplier</returns>
        [HttpPost(ApiRoutes.Suppliers.Create)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<SupplierDTO>> Create(SupplierDTO supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            supplier = await supplierService.CreateAsync(supplier);
            return CreatedAtAction(nameof(Get), new {supplierId = supplier.Id}, supplier);
        }
    }
}