using System.Threading;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Core.Commands.Supplier;
using Isitar.DoenerOrder.Core.Handlers.Supplier;
using Xunit;

namespace Isitar.DoenerOrder.Core.Tests.Supplier
{
    public class UpdateSupplierTests
    {
        [Fact]
        public async Task TestUpdateSupplierSuccess()
        {
            await using var context = DatabaseHelper.CreateInMemoryDatabaseContext(nameof(TestUpdateSupplierSuccess));

            await context.Suppliers.AddRangeAsync(new[]
            {
                new Data.DAO.Supplier
                {
                    Id = 1,
                    Name = "s1",
                    Email = "email@email.ch",
                    Phone = "+41 456 45 45"
                },
                new Data.DAO.Supplier
                {
                    Id = 2,
                    Name = "s2",
                    Email = "email@email.ch",
                    Phone = "+41 456 45 45"
                },
                new Data.DAO.Supplier
                {
                    Id = 3,
                    Name = "s3",
                    Email = "email@email.ch",
                    Phone = "+41 456 45 45"
                },
            });

            var updateCmd1 = new UpdateSupplierCommand
            {
                Id = 1,
                Name = "something",
                Email = "something@gmail.com",
                Phone = null,
            };
            var handler = new UpdateSupplierCommandHandler(context);
            var response1 = await handler.Handle(updateCmd1, CancellationToken.None);
            Assert.True(response1.Success);
            var updatedSupplier = context.Suppliers.Find(1);
            Assert.Equal(updateCmd1.Name, updatedSupplier.Name);
            Assert.Equal(updateCmd1.Email, updatedSupplier.Email);
            Assert.Equal(updateCmd1.Phone, updatedSupplier.Phone);
        }
    }
}