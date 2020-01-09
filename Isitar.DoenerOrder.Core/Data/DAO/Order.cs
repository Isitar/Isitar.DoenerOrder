using System.Collections.Generic;

namespace Isitar.DoenerOrder.Core.Data.DAO
{
    internal class Order
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }

        public bool Paid { get; set; }
        public IEnumerable<OrderLine> OrderLines { get; set; }
    }
}