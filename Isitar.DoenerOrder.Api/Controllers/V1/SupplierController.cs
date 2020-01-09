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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IAsyncEnumerable<SupplierDto>>> GetAll()
        {
            var query = new GetAllSuppliersQuery();
            var result = await mediator.Send(query);
            return result.Success
                ? (ActionResult<IAsyncEnumerable<SupplierDto>>) Ok(result.Data.Select(SupplierDto.FromCoreSupplierDTO))
                : BadRequest(result.ErrorMessages);
        }

        [HttpGet(ApiRoutes.Suppliers.Get)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SupplierDto>> Get(int supplierId)
        {
            var query = new GetSupplierByIdQuery {Id = supplierId};
            var result = await mediator.Send(query);
            return result.Success
                ? (ActionResult<SupplierDto>) Ok(SupplierDto.FromCoreSupplierDTO(result.Data))
                : NotFound(result.ErrorMessages);
        }


        /// <summary>
        /// Creates a supplier
        /// </summary>
        /// <param name="createSupplierViewModel"></param>
        /// <returns>the created supplier</returns>
        [HttpPost(ApiRoutes.Suppliers.Create)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<SupplierDto>> Create(CreateSupplierViewModel createSupplierViewModel)
        {
            var command = new CreateSupplierCommand
            {
                Name = createSupplierViewModel.Name,
                Email = createSupplierViewModel.Email,
                Phone = createSupplierViewModel.Phone
            };
            var result = await mediator.Send(command);
            return result.Success
                ? (ActionResult<SupplierDto>) CreatedAtAction(nameof(Get), new {supplierId = result.Data.Id},
                    SupplierDto.FromCoreSupplierDTO(result.Data))
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
        public async Task<ActionResult<SupplierDto>> Update(int supplierId,
            [FromBody] UpdateSupplierViewModel updateSupplierViewModel)
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
                ? (ActionResult<SupplierDto>) Ok(SupplierDto.FromCoreSupplierDTO(result.Data))
                : BadRequest(result.ErrorMessages);
        }

        [HttpGet(ApiRoutes.Suppliers.GetAllProducts)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IAsyncEnumerable<ProductDto>>> GetAllProducts(int supplierId)
        {
            var query = new GetAllProductsForSupplierQuery
            {
                SupplierId = supplierId
            };
            var result = await mediator.Send(query);
            return result.Success
                ? (ActionResult<IAsyncEnumerable<ProductDto>>) Ok(result.Data.Select(ProductDto.FromCoreProductDto))
                : BadRequest(result.ErrorMessages);
        }

        [HttpGet(ApiRoutes.Suppliers.GetProduct)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IAsyncEnumerable<ProductDto>>> GetProduct(int supplierId, int productId)
        {
            var query = new GetProductForSupplierByIdQuery
            {
                SupplierId = supplierId,
                ProductId = productId,
            };
            var result = await mediator.Send(query);
            return result.Success
                ? (ActionResult<IAsyncEnumerable<ProductDto>>) Ok(ProductDto.FromCoreProductDto(result.Data))
                : BadRequest(result.ErrorMessages);
        }

        [HttpPost(ApiRoutes.Suppliers.AddProduct)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ProductDto>> AddProduct(int supplierId,
            [FromBody] CreateProductViewModel createProductViewModel)
        {
            var command = new CreateProductForSupplierCommand
            {
                SupplierId = supplierId,
                Label = createProductViewModel.Label,
                Price = createProductViewModel.Price,
            };
            var result = await mediator.Send(command);
            return result.Success
                ? (ActionResult<ProductDto>) CreatedAtAction(nameof(GetProduct),
                    new {supplierId = result.Data.SupplierId, productId = result.Data.Id},
                    ProductDto.FromCoreProductDto(result.Data))
                : BadRequest(result.ErrorMessages);
        }
    }
}