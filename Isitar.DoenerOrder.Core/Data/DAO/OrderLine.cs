namespace Isitar.DoenerOrder.Core.DAO
{
    public class OrderLine
    {
        public int Id { get; set; }
        
        public Order Order { get; set; }
        
        public Ingredient Ingredient { get; set; }
    }
}