using System.Collections.Generic;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Api.Contracts.V1;
using Isitar.DoenerOrder.Api.Contracts.V1.Requests;
using Isitar.DoenerOrder.Api.Contracts.V1.Responses;
using Isitar.DoenerOrder.Api.Services;
using Isitar.DoenerOrder.Core.Commands.Supplier;
using Isitar.DoenerOrder.Core.Data;
using Isitar.DoenerOrder.Core.Queries.Supplier;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Isitar.DoenerOrder.Api.Controllers.V1
{
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
            return result.Success ? (ActionResult<IAsyncEnumerable<SupplierDTO>>) Ok(result.Data) : BadRequest(result.ErrorMessages);
        }
        
        [HttpGet(ApiRoutes.Suppliers.Get)]
        public async Task<ActionResult<SupplierDTO>> Get(int supplierId)
        {
            var query = new GetSupplierByIdQuery {Id = supplierId};
            var result = await mediator.Send(query);
            return result.Success ? (ActionResult<SupplierDTO>) Ok(result.Data) : BadRequest(result.ErrorMessages);
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
                    result.Data)
                : BadRequest(result.ErrorMessages);
        }
    }
}