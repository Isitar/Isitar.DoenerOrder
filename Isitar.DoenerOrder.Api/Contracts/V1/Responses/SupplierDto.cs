using Isitar.DoenerOrder.Core.Data.DAO;

namespace Isitar.DoenerOrder.Api.Contracts.V1.Responses

{
    public class SupplierDto 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        
        public static SupplierDto FromCoreSupplierDTO(Core.Responses.Supplier.SupplierDto s)
        {
            return new SupplierDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                Phone = s.Phone
            };
        }
    }
}