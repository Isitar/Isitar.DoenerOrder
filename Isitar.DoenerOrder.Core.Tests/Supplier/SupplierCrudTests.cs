using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Core.Commands.Supplier;
using Isitar.DoenerOrder.Core.Handlers.Supplier.CommandHandlers;
using Isitar.DoenerOrder.Core.Handlers.Supplier.QueryHandlers;
using Isitar.DoenerOrder.Core.Queries.Supplier;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Isitar.DoenerOrder.Core.Tests.Supplier
{
    public class SupplierCrudTests
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
            var supplierId = cmdResponse.Data;
            Assert.True(cmdResponse.Success);
            var queryOneHandler = new GetSupplierByIdQueryHandler(context);
            var queryOne = new GetSupplierByIdQuery {Id = supplierId};
            var queryOneResponse = await queryOneHandler.Handle(queryOne, CancellationToken.None);
            Assert.True(queryOneResponse.Success);
            Assert.Equal(supplierId, queryOneResponse.Data.Id);
            Assert.Equal(cmd.Email, queryOneResponse.Data.Email);
            Assert.Equal(cmd.Name, queryOneResponse.Data.Name);
            Assert.Equal(cmd.Phone, queryOneResponse.Data.Phone);

            var queryAll = new GetAllSuppliersQuery();
            var queryAllHandler = new GetAllSuppliersQueryHandler(context);
            var queryAllResponse = await queryAllHandler.Handle(queryAll, CancellationToken.None);
            Assert.True(queryAllResponse.Success);
            Assert.NotEmpty(queryAllResponse.Data);
            Assert.Contains(queryAllResponse.Data, s => s.Id.Equals(supplierId));
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

        [Theory]
        [InlineData("salidu")]
        [InlineData("salidu@")]
        [InlineData("@salidu")]
        [InlineData("sali du@gmail.com")]
        [InlineData(";du@gmail.com")]
        [InlineData("saldu;@gmail.com")]
        [InlineData("saldu@gma;il.com")]
        [InlineData("saldu@gmail.c;om")]
        public async Task ValidateEmail(string email)
        {
            var createSupplierCommand = new CreateSupplierCommand
            {
                Name = "Pascal",
                Phone = "+41 79 456 45 45",
                Email = email,
            };

            var createSupplierCommandValidator = new CreateSupplierCommandValidator();
            var result = await createSupplierCommandValidator.ValidateAsync(createSupplierCommand);
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(createSupplierCommand.Email));
            Assert.DoesNotContain(result.Errors, e => e.PropertyName == nameof(createSupplierCommand.Name));
            Assert.DoesNotContain(result.Errors, e => e.PropertyName == nameof(createSupplierCommand.Phone));

            var updateSupplierCommand = new UpdateSupplierCommand
            {
                Id = 1,
                Name = "Pascal",
                Phone = "+41 79 456 45 45",
                Email = email,
            };
            var updateSupplierCommandValidator = new UpdateSupplierCommandValidator();
            result = await updateSupplierCommandValidator.ValidateAsync(updateSupplierCommand);
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(updateSupplierCommand.Email));
            Assert.DoesNotContain(result.Errors, e => e.PropertyName == nameof(updateSupplierCommand.Id));
            Assert.DoesNotContain(result.Errors, e => e.PropertyName == nameof(updateSupplierCommand.Name));
            Assert.DoesNotContain(result.Errors, e => e.PropertyName == nameof(updateSupplierCommand.Phone));
        }


        [Fact]
        public async Task TestUpdateSupplierSuccess()
        {
            await using var context = DatabaseHelper.CreateInMemoryDatabaseContext(nameof(TestUpdateSupplierSuccess));

            var addedSupplier = await context.Suppliers.AddAsync(ValidModelCreator.Supplier());
            var supplier1Id = addedSupplier.Entity.Id;

            var updateCmd1 = new UpdateSupplierCommand
            {
                Id = supplier1Id,
                Name = "something",
                Email = "something@gmail.com",
                Phone = null,
            };

            var updateHandler = new UpdateSupplierCommandHandler(context);
            var queryOneHandler = new GetSupplierByIdQueryHandler(context);
            var response1 = await updateHandler.Handle(updateCmd1, CancellationToken.None);
            Assert.True(response1.Success);
            var updatedSupplier =
                (await queryOneHandler.Handle(new GetSupplierByIdQuery {Id = updateCmd1.Id}, CancellationToken.None))
                .Data;
            Assert.Equal(updateCmd1.Name, updatedSupplier.Name);
            Assert.Equal(updateCmd1.Email, updatedSupplier.Email);
            Assert.Equal(updateCmd1.Phone, updatedSupplier.Phone);
            var updateCmd2 = new UpdateSupplierCommand
            {
                Id = supplier1Id,
                Name = "something new",
                Email = "validMail@mail.ch",
                Phone = "+41 79 456 45 45",
            };

            var response2 = await updateHandler.Handle(updateCmd2, CancellationToken.None);
            Assert.True(response2.Success);
            var updatedAgain =
                (await queryOneHandler.Handle(new GetSupplierByIdQuery {Id = updateCmd1.Id}, CancellationToken.None))
                .Data;
            Assert.Equal(updateCmd2.Name, updatedAgain.Name);
            Assert.Equal(updateCmd2.Email, updatedAgain.Email);
            Assert.Equal(updateCmd2.Phone, updatedAgain.Phone);

            var updateNonExistingSupplierCommand = new UpdateSupplierCommand
            {
                Id = supplier1Id + 1,
                Name = "something",
                Email = "sali@valid.ch",
                Phone = "+41 79 456 45 45",
            };
            var response3 = await updateHandler.Handle(updateNonExistingSupplierCommand, CancellationToken.None);
            Assert.False(response3.Success);
        }

        [Fact]
        public async Task DeleteSupplier()
        {
            await using var context = DatabaseHelper.CreateInMemoryDatabaseContext(nameof(DeleteSupplier));
            var supplierId1Task= context.AddAsync(ValidModelCreator.Supplier());
            var supplierId2Task = context.AddAsync(ValidModelCreator.Supplier());
            var supplierId3Task  = context.AddAsync(ValidModelCreator.Supplier());

            var supplier1Id = (await supplierId1Task).Entity.Id;
            var supplier2Id = (await supplierId2Task).Entity.Id;
            var supplier3Id = (await supplierId3Task).Entity.Id;
            var deleteSupplier2Cmd = new DeleteSupplierCommand
            {
                Id = supplier2Id,
            };
            var deleteHandler = new DeleteSupplierCommandHandler(context);
            var deleteResult1 = await deleteHandler.Handle(deleteSupplier2Cmd, CancellationToken.None);
            Assert.True(deleteResult1.Success);
            var querySupplier2 = new GetSupplierByIdQuery {Id = supplier2Id};
            var queryOneHandler = new GetSupplierByIdQueryHandler(context);
            var querySupplier2Res = await queryOneHandler.Handle(querySupplier2, CancellationToken.None);
            Assert.False(querySupplier2Res.Success);

            var deleteResult2 = await deleteHandler.Handle(deleteSupplier2Cmd, CancellationToken.None);
            Assert.False(deleteResult2.Success);
            
            var querySupplier1 = new GetSupplierByIdQuery {Id = supplier1Id};
            var query1Result = await queryOneHandler.Handle(querySupplier1, CancellationToken.None);
            Assert.True(query1Result.Success);
            var querySupplier3 = new GetSupplierByIdQuery {Id = supplier3Id};
            var query3Result = await queryOneHandler.Handle(querySupplier3, CancellationToken.None);
            Assert.True(query3Result.Success);
            
            
        }
    }
}