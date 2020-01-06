namespace Isitar.DoenerOrder.Core.Data.DAO
{
    public class ProductIngredient
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public decimal AdditionalCost { get; set; }
    }
}