using Isitar.DoenerOrder.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Isitar.DoenerOrder.Core.Tests
{
    public class DatabaseHelper
    {
        public static DoenerOrderContext CreateInMemoryDatabaseContext(string name)
        {
            
            var options = new DbContextOptionsBuilder<DoenerOrderContext>()
                .UseInMemoryDatabase(name)
                .Options;
            return new DoenerOrderContext(options);
        } 
    }
}