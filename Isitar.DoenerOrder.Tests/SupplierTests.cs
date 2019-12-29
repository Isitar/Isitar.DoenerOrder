using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Isitar.DoenerOrder.Contracts.Requests;
using Xunit;

namespace Isitar.DoenerOrder.Tests
{
    public class SupplierTests
    {
        [Theory]
        [InlineData("salidu")]
        [InlineData("salidu@")]
        [InlineData("@salidu")]
        public void ValidateEmail(string email)
        {
            var supplier = new SupplierDTO();
            supplier.Name = "Pascal";
            supplier.Email = email;
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(supplier);
            var result = Validator.TryValidateObject(supplier,context, validationResults);
            Assert.False(result, $"Email {email} should not be valid");
        }
    }
}