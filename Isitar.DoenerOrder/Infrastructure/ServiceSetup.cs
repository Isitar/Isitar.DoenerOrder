using Isitar.DoenerOrder.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Isitar.DoenerOrder.Infrastructure
{
    public static class ServiceSetup
    {
        public static void ConfigureService(IServiceCollection services)
        {
            services.AddScoped<IIdentityService, IdentityService>();
        }
    }
}