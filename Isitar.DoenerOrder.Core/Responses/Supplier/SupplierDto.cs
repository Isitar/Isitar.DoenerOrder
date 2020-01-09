namespace Isitar.DoenerOrder.Core.Responses.Supplier
{
    public class SupplierDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        internal static SupplierDto FromSupplier(Data.DAO.Supplier supplier)
        {
            return new SupplierDto
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Email = supplier.Email,
                Phone = supplier.Phone,
            };
        }
    }
}