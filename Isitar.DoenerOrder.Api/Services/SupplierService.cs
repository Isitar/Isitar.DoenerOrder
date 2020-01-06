using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Api.Contracts.V1.Responses;
using Isitar.DoenerOrder.Core.Data;
using Isitar.DoenerOrder.Core.Data.DAO;
using Microsoft.EntityFrameworkCore;

namespace Isitar.DoenerOrder.Api.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly DoenerOrderContext dbContext;

        public SupplierService(DoenerOrderContext dbContext)
        {
            this.dbContext = dbContext;
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

        public async Task<SupplierDTO> CreateAsync(string name, string email, string? phone)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
            {
                return null;
            }
            
            var supplier = await dbContext.Suppliers.AddAsync(new Supplier
            {
                Name = name.Trim(),
                Email = email.Trim(),
                Phone = phone?.Trim()
            });
            await dbContext.SaveChangesAsync();
            return SupplierDTO.FromSupplier(supplier.Entity);
        }
        
        public async Task<SupplierDTO> UpdateAsync(int id, string? name, string? email, string? phone)
        {
            var supplier = await dbContext.FindAsync<Supplier>(id);
            if (null == supplier)
            {
                throw new ArgumentException("Supplier not found", nameof(id));
            }

            if (null != name)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return null;
                }
                supplier.Name = name.Trim();
            }
            if (null != email)
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return null;
                }
                supplier.Email = email.Trim();
            }
            if (null != phone)
            {
                supplier.Phone = string.IsNullOrEmpty(phone) ? null : phone.Trim();
            }

            var updateResult = dbContext.Suppliers.Update(supplier);
            await dbContext.SaveChangesAsync();
            return SupplierDTO.FromSupplier(updateResult.Entity);
        }
        
    }
}