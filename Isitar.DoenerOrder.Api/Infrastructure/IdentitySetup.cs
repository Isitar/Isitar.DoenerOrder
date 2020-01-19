using Isitar.DoenerOrder.Auth.Data;
using Isitar.DoenerOrder.Auth.Data.DAO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Isitar.DoenerOrder.Api.Infrastructure
{
    public static class IdentitySetup
    {
        public static void ConfigureService(IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;

                    options.Password.RequiredLength = 8;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireDigit = true;
                })
                .AddEntityFrameworkStores<AppIdentityDbContext>();
        }

        public static void ConfigureApplication(IApplicationBuilder app)
        {
        }
    }
}