using System.Collections.Generic;

namespace Isitar.DoenerOrder.Core.Data.DAO
{
    internal class Ingredient
    {
        public int Id { get; set; }
        public string Label { get; set; }

        public string FunnyLabel { get; set; }

        public IList<ProductIngredient> ProductIngredients { get; set; }
    }
}