using System.Collections.Generic;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Api.Contracts.V1;
using Isitar.DoenerOrder.Api.Contracts.V1.Requests;
using Isitar.DoenerOrder.Api.Contracts.V1.Responses;
using Isitar.DoenerOrder.Api.Services;
using Isitar.DoenerOrder.Core.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Isitar.DoenerOrder.Api.Controllers.V1
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
        /// <param name="createSupplierViewModel"></param>
        /// <returns>the created supplier</returns>
        [HttpPost(ApiRoutes.Suppliers.Create)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<SupplierDTO>> Create(CreateSupplierViewModel createSupplierViewModel)
        {
            var supplier = await supplierService.CreateAsync(createSupplierViewModel.Name, createSupplierViewModel.Email, createSupplierViewModel.Phone);
            if (null == supplier)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Get), new {supplierId = supplier.Id}, supplier);
        }

        /// <summary>
        /// Updates a supplier
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="updateSupplierView"></param>
        /// <returns>the updated supplier</returns>
        [HttpPut(ApiRoutes.Suppliers.Update)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<SupplierDTO>> Create(int supplierId, UpdateSupplierViewModel updateSupplierView)
        {
            var supplier = await supplierService.UpdateAsync(supplierId, updateSupplierView.Name, updateSupplierView.Email, updateSupplierView.Phone);
            if (null == supplier)
            {
                return BadRequest();
            }
            return Ok(supplier);
        }
    }
}