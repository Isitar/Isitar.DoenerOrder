using System.ComponentModel.DataAnnotations;
using Isitar.DoenerOrder.Domain.DAO;

namespace Isitar.DoenerOrder.Contracts.Requests
{
    public class SupplierDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
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