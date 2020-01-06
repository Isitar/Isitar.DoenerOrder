using Isitar.DoenerOrder.Api.Core.Domain.DAO;

namespace Isitar.DoenerOrder.Api.Contracts.V1.Responses

{
    public class SupplierDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public static SupplierDTO FromSupplier(Supplier s)
        {
            return new SupplierDTO
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                Phone = s.Phone
            };
        }
    }
}