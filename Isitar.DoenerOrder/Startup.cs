using Isitar.DoenerOrder.Data;
using Isitar.DoenerOrder.Infrastructure;
using Isitar.DoenerOrder.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Isitar.DoenerOrder
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<DoenerOrderContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });

            IdentitySetup.ConfigureService(services);
            JwtSetup.ConfigureService(services, Configuration);
            SwaggerSetup.ConfigureService(services);
            ServiceSetup.ConfigureService(services);

            services.AddCors(options =>
                options.AddPolicy("AllowAll", builder => { builder.WithOrigins("*").AllowAnyMethod(); })
            );

            services.AddScoped<SupplierService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DoenerOrderContext dbContext)
        {
            dbContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SwaggerSetup.ConfigureApplication(app);
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            JwtSetup.ConfigureApplication(app);
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}