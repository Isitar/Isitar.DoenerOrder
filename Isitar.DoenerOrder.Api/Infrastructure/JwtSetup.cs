using System.Reflection;
using System.Text;
using Isitar.DoenerOrder.Api.Helpers.Auth;
using Isitar.DoenerOrder.Auth.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Isitar.DoenerOrder.Api.Infrastructure
{
    public static class JwtSetup
    {
        public static void ConfigureService(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(JwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                ValidateLifetime = true,
                RequireExpirationTime = true,
            };
            services.AddSingleton(tokenValidationParameters);
            
            services.AddAuthentication(config =>
                {
                    config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddCookie(options => { options.SlidingExpiration = true; })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                    options => { options.TokenValidationParameters = tokenValidationParameters; }
                );

            services.AddAuthorization(options =>
            {
                foreach (var prop in typeof(ClaimPermission).GetFields(
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
                {
                    var propertyValue = prop.GetValue(null).ToString();
                    options.AddPolicy(propertyValue,
                        policy => policy.RequireClaim(CustomClaimTypes.Permission, propertyValue));
                }
            });
        }

        public static void ConfigureApplication(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}