namespace Isitar.DoenerOrder.Api.Contracts.V1.Requests
{
    public class UpdateSupplierViewModel
    {
        public string Name { get; set; }
        
        public string? Email { get; set; }
        
        public string? Phone { get; set; }
    }
}