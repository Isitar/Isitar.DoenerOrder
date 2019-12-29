using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Contracts.Requests;
using Isitar.DoenerOrder.Data;
using Isitar.DoenerOrder.Domain;
using Isitar.DoenerOrder.Domain.DAO;
using Microsoft.EntityFrameworkCore;

namespace Isitar.DoenerOrder.Services
{
    public class SupplierService : Services.BusinessLayer
    {
        public SupplierService(DoenerOrderContext dbContext) : base(dbContext)
        {
        }

        public async Task<SupplierDTO> GetAsync(int id)
        {
            var supplier = await dbContext.FindAsync<Supplier>(id);
            return SupplierDTO.FromSupplier(supplier);
        }

        public async Task<IEnumerable<SupplierDTO>> GetAllAsync()
        {
            return await dbContext.Suppliers.Select(s => SupplierDTO.FromSupplier(s)).ToListAsync();
        }

        public async Task<SupplierDTO> CreateAsync(SupplierDTO supplierDTO)
        {
            Validator.ValidateObject(supplierDTO, new ValidationContext(supplierDTO, null, null), true);
            var supplier = await dbContext.Suppliers.AddAsync(new Supplier
            {
                Name = supplierDTO.Name,
                Email = supplierDTO.Email,
                Phone = supplierDTO.Phone
            });
            await dbContext.SaveChangesAsync();
            return SupplierDTO.FromSupplier(supplier.Entity);
        }
        
        public async Task<SupplierDTO> UpdateAsync(int id, SupplierDTO supplierDTO)
        {
            Validator.ValidateObject(supplierDTO, new ValidationContext(supplierDTO, null, null), true);
            var originalSupplier = await dbContext.FindAsync<Supplier>(id);
            if (null == originalSupplier)
            {
                throw new ArgumentException("Supplier not found", nameof(id));
            }

            originalSupplier.Name = supplierDTO.Name;
            originalSupplier.Email = supplierDTO.Email;
            originalSupplier.Phone = supplierDTO.Phone;

            var supplier = dbContext.Suppliers.Update(originalSupplier);
            await dbContext.SaveChangesAsync();
            return SupplierDTO.FromSupplier(supplier.Entity);
        }
        
    }
}