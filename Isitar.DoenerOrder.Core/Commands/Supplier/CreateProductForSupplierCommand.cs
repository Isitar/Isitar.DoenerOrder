namespace Isitar.DoenerOrder.Core.Commands.Supplier
{
    public class CreateProductForSupplierCommand
    {
        public int SupplierId { get; set; }
        public string Label { get; set; }
        public decimal Price { get; set; }
    }
}