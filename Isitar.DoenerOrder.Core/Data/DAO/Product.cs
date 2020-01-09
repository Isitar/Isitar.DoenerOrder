using System.Collections.Generic;

namespace Isitar.DoenerOrder.Core.Data.DAO
{
    internal class Product
    {
        public int Id { get; set; }
        
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public string Label { get; set; }
        public decimal Price { get; set; }

        public IList<ProductIngredient> ProductIngredients { get; set; }
    }
}