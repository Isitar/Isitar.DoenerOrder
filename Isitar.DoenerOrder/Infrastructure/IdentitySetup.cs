using Isitar.DoenerOrder.Data;
using Isitar.DoenerOrder.Domain;
using Isitar.DoenerOrder.Domain.DAO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Isitar.DoenerOrder.Infrastructure
{
    public static class IdentitySetup
    {
        public static void ConfigureService(IServiceCollection services)
        {
            services.AddIdentity<User, Role>(options =>
                {
                    options.User.RequireUniqueEmail = true;

                    options.Password.RequiredLength = 8;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireDigit = true;
                })
                .AddEntityFrameworkStores<DoenerOrderContext>();
        }

        public static void ConfigureApplication(IApplicationBuilder app)
        {
        }
    }
}