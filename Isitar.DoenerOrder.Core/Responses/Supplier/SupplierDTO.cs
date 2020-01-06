namespace Isitar.DoenerOrder.Core.Responses.Supplier
{
    public class SupplierDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public static SupplierDTO FromSupplier(Data.DAO.Supplier supplier)
        {
            return new SupplierDTO
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Email = supplier.Email,
                Phone = supplier.Phone,
            };
        }
    }
}