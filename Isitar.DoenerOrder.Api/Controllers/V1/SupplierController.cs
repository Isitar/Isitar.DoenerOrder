using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Api.Contracts.V1;
using Isitar.DoenerOrder.Api.Contracts.V1.Requests;
using Isitar.DoenerOrder.Api.Contracts.V1.Responses;
using Isitar.DoenerOrder.Core.Commands.Supplier;
using Isitar.DoenerOrder.Core.Data;
using Isitar.DoenerOrder.Core.Queries.Supplier;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Isitar.DoenerOrder.Api.Controllers.V1
{
    /// <summary>
    /// Supplier controller to manage supplier resources
    /// </summary>
    public class SupplierController : ApiController
    {
        private readonly IMediator mediator;

        public SupplierController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        [HttpGet(ApiRoutes.Suppliers.GetAll)]
        public async Task<ActionResult<IAsyncEnumerable<SupplierDTO>>> GetAll()
        {
            var query = new GetAllSuppliersQuery();
            var result = await mediator.Send(query);
            return result.Success ? (ActionResult<IAsyncEnumerable<SupplierDTO>>) Ok(result.Data.Select(SupplierDTO.FromCoreSupplierDTO)) : BadRequest(result.ErrorMessages);
        }
        
        [HttpGet(ApiRoutes.Suppliers.Get)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SupplierDTO>> Get(int supplierId)
        {
            var query = new GetSupplierByIdQuery {Id = supplierId};
            var result = await mediator.Send(query);
            return result.Success ? (ActionResult<SupplierDTO>) Ok(SupplierDTO.FromCoreSupplierDTO(result.Data)) : NotFound(result.ErrorMessages);
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
            var command = new CreateSupplierCommand
            {
                Name = createSupplierViewModel.Name,
                Email = createSupplierViewModel.Email,
                Phone = createSupplierViewModel.Phone
            };
            var result = await mediator.Send(command);
            return result.Success
                ? (ActionResult<SupplierDTO>) CreatedAtAction(nameof(Get), new {supplierId = result.Data.Id},
                    SupplierDTO.FromCoreSupplierDTO(result.Data))
                : BadRequest(result.ErrorMessages);
        }

        
        /// <summary>
        /// Updates a supplier, keep in mind all properties will be updated
        /// </summary>
        /// <param name="supplierId">the id of the supplier to update</param>
        /// <param name="updateSupplierViewModel">how to update the supplier</param>
        /// <returns></returns>
        [HttpPut(ApiRoutes.Suppliers.Update)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<SupplierDTO>> Update(int supplierId, UpdateSupplierViewModel updateSupplierViewModel)
        {
            var command = new UpdateSupplierCommand
            {
                Id = supplierId,
                Name = updateSupplierViewModel.Name,
                Email = updateSupplierViewModel.Email,
                Phone = updateSupplierViewModel.Phone
            };
            var result = await mediator.Send(command);
            return result.Success
                ? (ActionResult<SupplierDTO>) Ok(SupplierDTO.FromCoreSupplierDTO(result.Data))
                : BadRequest(result.ErrorMessages);
        }
    }
}