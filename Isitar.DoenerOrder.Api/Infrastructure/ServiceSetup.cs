using Isitar.DoenerOrder.Auth.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Isitar.DoenerOrder.Api.Infrastructure
{
    public static class ServiceSetup
    {
        public static void ConfigureService(IServiceCollection services)
        {
            services.AddScoped<IIdentityService, IdentityService>();
        }
    }
}