using System;
using System.Collections.Generic;

namespace Isitar.DoenerOrder.Domain.DAO
{
    public class BulkOrder
    {
        public int Id { get; set; }
        
        public User CreationUser { get; set; }

        public DateTime Deadline { get; set; }

        public Supplier Supplier { get; set; }

        public IList<Order> Orders { get; set; }
    }
}