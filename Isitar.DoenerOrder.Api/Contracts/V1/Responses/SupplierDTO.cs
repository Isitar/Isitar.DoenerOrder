using Isitar.DoenerOrder.Core.Data.DAO;

namespace Isitar.DoenerOrder.Api.Contracts.V1.Responses

{
    public class SupplierDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public static SupplierDTO FromCoreSupplierDTO(Core.Responses.Supplier.SupplierDTO s)
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