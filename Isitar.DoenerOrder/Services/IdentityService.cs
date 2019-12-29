using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Isitar.DoenerOrder.Contracts.Requests;
using Isitar.DoenerOrder.Domain;
using Isitar.DoenerOrder.Domain.DAO;
using Isitar.DoenerOrder.Domain.Responses;
using Isitar.DoenerOrder.Options;
using Microsoft.IdentityModel.Tokens;

namespace Isitar.DoenerOrder.Services
{
    public class IdentityService : IIdentityService
    {
        private UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly JwtSettings jwtSettings;

        public IdentityService(UserManager<User> userManager, RoleManager<Role> roleManager, JwtSettings jwtSettings)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.jwtSettings = jwtSettings;
        }

        public async Task<AuthResponse> LoginAsync(LoginViewModel loginViewModel)
        {
            var user = await userManager.FindByNameAsync(loginViewModel.Username);
            var res = await userManager.CheckPasswordAsync(user, loginViewModel.Password);
            if (!res)
            {
                return new AuthResponse
                {
                    Message = "Username / Password wrong"
                };
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
            var singingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.Add(jwtSettings.TokenLifetime);
            var claims = await GetValidClaims(user);

            var token = new JwtSecurityToken(
                jwtSettings.Issuer,
                jwtSettings.Audience,
                claims,
                expires: expiry,
                signingCredentials: singingCredentials
            );

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Success = true
            };
        }

        public async Task<bool> RegisterAsync(RegistrationViewModel registrationViewModel)
        {
            var res = await userManager.CreateAsync(new User
                {
                    UserName = registrationViewModel.Username,
                    Email = registrationViewModel.Email,
                },
                registrationViewModel.Password);
            return res.Succeeded;
        }

        private async Task<IEnumerable<Claim>> GetValidClaims(User user)
        {
            var identityOptions = new IdentityOptions();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64),
                new Claim(identityOptions.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
                new Claim(identityOptions.ClaimsIdentity.UserNameClaimType, user.UserName),
            };
            var userClaims = await userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await roleManager.GetClaimsAsync(role);
                    claims.AddRange(roleClaims);
                }
            }

            return claims;
        }
    }
}