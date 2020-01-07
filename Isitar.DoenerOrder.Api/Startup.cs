using FluentValidation;
using Isitar.DoenerOrder.Api.Infrastructure;
using Isitar.DoenerOrder.Auth.Data;
using Isitar.DoenerOrder.Core.Behaviors;
using Isitar.DoenerOrder.Core.Commands.Supplier;
using Isitar.DoenerOrder.Core.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Isitar.DoenerOrder.Api
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
            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("IdentityConnection"));
            });

            IdentitySetup.ConfigureService(services);
            JwtSetup.ConfigureService(services, Configuration);
            SwaggerSetup.ConfigureService(services);
            
            services.AddMediatR(typeof(CreateSupplierCommand).Assembly);
            services.AddValidatorsFromAssembly(typeof(CreateSupplierCommand).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            
            ServiceSetup.ConfigureService(services);

            services.AddCors(options =>
                options.AddPolicy("AllowAll", builder => { builder.WithOrigins("*").AllowAnyMethod(); })
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DoenerOrderContext dbContext, AppIdentityDbContext appIdentityDbContext)
        {
            dbContext.Database.Migrate();
            appIdentityDbContext.Database.Migrate();

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