namespace Isitar.DoenerOrder.Core.Responses.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string Label { get; set; }
        public decimal Price { get; set; }

        internal static ProductDto FromProduct(Data.DAO.Product productEntity)
        {
            return new ProductDto
            {
                Id = productEntity.Id,
                Label = productEntity.Label,
                Price = productEntity.Price,
                SupplierId = productEntity.SupplierId,
            };
        }
    }
}