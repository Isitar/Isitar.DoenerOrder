using System.Collections.Generic;

namespace Isitar.DoenerOrder.Core.Data.DAO
{
    internal class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public IList<Product> Products { get; set; }
    }
}