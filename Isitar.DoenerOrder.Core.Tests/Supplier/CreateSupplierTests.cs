using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Core.Commands.Supplier;
using Isitar.DoenerOrder.Core.Data;
using Isitar.DoenerOrder.Core.Handlers.Supplier;
using Isitar.DoenerOrder.Core.Queries.Supplier;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Isitar.DoenerOrder.Core.Tests.Supplier
{
    public class CreateSupplierTests
    {
        [Fact]
        public async Task TestCreateSuccess()
        {

            var cmd = new CreateSupplierCommand
            {
                Email = "somemail@gmail.com",
                Name = "Pascal",
                Phone = "+41 79 456 45 45"
            };

            await using var context = DatabaseHelper.CreateInMemoryDatabaseContext(nameof(TestCreateSuccess));
            var cmdHandler = new CreateSupplierCommandHandler(context);
            var cmdResponse = await cmdHandler.Handle(cmd, CancellationToken.None);
            Assert.True(cmdResponse.Success);
            Assert.Equal(cmd.Email, cmdResponse.Data.Email);
            Assert.Equal(cmd.Name, cmdResponse.Data.Name);
            Assert.Equal(cmd.Phone, cmdResponse.Data.Phone);
            Assert.NotEqual(0, cmdResponse.Data.Id);

            var queryOne = new GetSupplierByIdQuery {Id = cmdResponse.Data.Id};
            var queryOneHandler = new GetSupplierByIdQueryHandler(context);
            var queryOneResponse = await queryOneHandler.Handle(queryOne, CancellationToken.None);
            Assert.True(queryOneResponse.Success);
            Assert.Equal(cmdResponse.Data.Id, queryOneResponse.Data.Id);
            Assert.Equal(cmd.Email, queryOneResponse.Data.Email);
            Assert.Equal(cmd.Name, queryOneResponse.Data.Name);
            Assert.Equal(cmd.Phone, queryOneResponse.Data.Phone);
            
            var queryAll = new GetAllSuppliersQuery();
            var queryAllHandler = new GetAllSuppliersQueryHandler(context);
            var queryAllResponse = await queryAllHandler.Handle(queryAll, CancellationToken.None);
            Assert.True(queryAllResponse.Success);
            Assert.NotEmpty(queryAllResponse.Data);
            Assert.Contains(queryAllResponse.Data, s => s.Id.Equals(cmdResponse.Data.Id));

        }

        [Fact]
        public async Task CreateSupplierValidation()
        {
            var validator = new CreateSupplierCommandValidator();

            var cmdValid = new CreateSupplierCommand
            {
                Name = "Sali",
                Email = "validMail@gmail.com",
                Phone = "+41 79 456 45 45",
            };
            var resultValid = await validator.ValidateAsync(cmdValid);
            Assert.True(resultValid.IsValid);

            var cmdInvalidMail = new CreateSupplierCommand
            {
                Email = "not valid mail",
                Name = "some name",
                Phone = "+41 79 456 45 45",
            };
            var resultInvalidMail = await validator.ValidateAsync(cmdInvalidMail);
            Assert.False(resultInvalidMail.IsValid);
            Assert.NotEmpty(resultInvalidMail.Errors);
            Assert.Contains(resultInvalidMail.Errors, e => e.PropertyName == nameof(cmdInvalidMail.Email));
            Assert.DoesNotContain(resultInvalidMail.Errors, e => e.PropertyName == nameof(cmdInvalidMail.Name));
            Assert.DoesNotContain(resultInvalidMail.Errors, e => e.PropertyName == nameof(cmdInvalidMail.Phone));
            
            var cmdMissingName = new CreateSupplierCommand
            {
                Email = "someValidMail@gmail.com",
                Phone = "+41 79 456 45 45",
            };
            var resultMissingName = await validator.ValidateAsync(cmdMissingName);
            Assert.False(resultMissingName.IsValid);
            Assert.Contains(resultMissingName.Errors, e => e.PropertyName == nameof(cmdMissingName.Name));
            Assert.DoesNotContain(resultMissingName.Errors, e => e.PropertyName == nameof(cmdMissingName.Email));
            Assert.DoesNotContain(resultMissingName.Errors, e => e.PropertyName == nameof(cmdMissingName.Phone));

        }
    }
}