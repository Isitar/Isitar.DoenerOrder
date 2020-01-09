namespace Isitar.DoenerOrder.Api.Contracts.V1.Responses
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public decimal Price { get; set; }
        
        public static ProductDto FromCoreProductDto(Core.Responses.Product.ProductDto p)
        {
            return new ProductDto
            {
                Id = p.Id,
                Label = p.Label,
                Price = p.Price,
            };
        }
    }
}