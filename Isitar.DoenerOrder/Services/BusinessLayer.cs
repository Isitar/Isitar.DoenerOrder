using Isitar.DoenerOrder.Data;

namespace Isitar.DoenerOrder.Services
{
    public class BusinessLayer
    {
        protected readonly DoenerOrderContext dbContext;

        public BusinessLayer(DoenerOrderContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}