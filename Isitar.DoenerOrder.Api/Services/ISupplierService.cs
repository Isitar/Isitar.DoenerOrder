using System.Collections.Generic;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Api.Contracts.V1.Responses;

namespace Isitar.DoenerOrder.Api.Services
{
    public interface ISupplierService
    {
        public Task<SupplierDTO> GetAsync(int id);
        public Task<IEnumerable<SupplierDTO>> GetAllAsync();
        public Task<SupplierDTO> CreateAsync(string name, string email, string? phone);
        public Task<SupplierDTO> UpdateAsync(int id, string? name, string? email, string? phone);
    }
}