using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Auth.Data;
using Isitar.DoenerOrder.Auth.Data.DAO;
using Isitar.DoenerOrder.Auth.Options;
using Isitar.DoenerOrder.Auth.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Isitar.DoenerOrder.Auth.Services
{
    public class IdentityService : IIdentityService
    {
        private UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly JwtSettings jwtSettings;
        private readonly TokenValidationParameters tokenValidationParameters;
        private readonly AppIdentityDbContext dbContext;

        private const string JwtUserIdClaimName = "isitar.ch/user_id";

        public IdentityService(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            JwtSettings jwtSettings,
            TokenValidationParameters tokenValidationParameters,
            AppIdentityDbContext dbContext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.jwtSettings = jwtSettings;
            this.tokenValidationParameters = tokenValidationParameters;
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Generates a AuthResponse based on existing login data
        /// </summary>
        /// <param name="username">the username</param>
        /// <param name="password">the unhashed password</param>
        /// <returns>An AuthResponse with either the token inside or an error message</returns>
        public async Task<AuthResponse> LoginAsync(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null
                || !await userManager.CheckPasswordAsync(user, password)
            )
            {
                return new AuthResponse
                {
                    Message = "Username / Password wrong"
                };
            }

            return await GenerateAuthenticationResultForUserAsync(user);
        }

        /// <summary>
        /// Registers a new user with the given parameters
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>true if everything worked, false otherwise</returns>
        public async Task<bool> RegisterAsync(string username, string email, string password)
        {
            var res = await userManager.CreateAsync(new AppUser
                {
                    UserName = username,
                    Email = email,
                },
                password);
            return res.Succeeded;
        }

        /// <summary>
        /// Generates a new AuthResponse with the given refreshToken and jwtToken
        /// </summary>
        /// <param name="refreshTokenString">the refresh token</param>
        /// <param name="jwtToken">the jwt token</param>
        /// <returns>A new AuthResponse containing either the tokens or error messages</returns>
        public async Task<AuthResponse> RefreshAsync(string refreshTokenString, string jwtToken)
        {
            var errorResponse = new AuthResponse
            {
                Message = "Invalid token"
            };

            var validatedToken = GetPrincipalFromToken(jwtToken);
            if (null == validatedToken)
            {
                return errorResponse;
            }

            var expiryDateUnix =
                long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                return errorResponse;
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken =
                await dbContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshTokenString);
            var userId = int.Parse(validatedToken.Claims.Single(x => x.Type == JwtUserIdClaimName).Value);

            if (storedRefreshToken == null
                || DateTime.UtcNow > storedRefreshToken.Expires
                || storedRefreshToken.Invalidated
                || storedRefreshToken.Used
                || storedRefreshToken.JwtTokenId != jti
                || userId != storedRefreshToken.UserId
            )
            {
                return errorResponse;
            }

            storedRefreshToken.Used = true;
            dbContext.RefreshTokens.Update(storedRefreshToken);
            await dbContext.SaveChangesAsync();

            var user = await userManager.FindByIdAsync(userId.ToString());
            return await GenerateAuthenticationResultForUserAsync(user);
        }

        /// <summary>
        /// Generates the Token for the given user. Saves the refresh token in the database
        /// </summary>
        /// <param name="user">the user the token should be generated for</param>
        /// <returns>The AuthResponse with the token</returns>
        private async Task<AuthResponse> GenerateAuthenticationResultForUserAsync(AppUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
            var singingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.Add(jwtSettings.TokenLifetime);
            var claims = await GetValidClaimsAsync(user);

            var token = new JwtSecurityToken(
                jwtSettings.Issuer,
                jwtSettings.Audience,
                claims,
                expires: expiry,
                signingCredentials: singingCredentials
            );

            var refreshToken = new RefreshToken
            {
                JwtTokenId = token.Id,
                Token = Guid.NewGuid().ToString(),
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddMonths(3)
            };

            await dbContext.RefreshTokens.AddAsync(refreshToken);
            await dbContext.SaveChangesAsync();

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken.Token,
                Success = true
            };
        }

        private async Task<IEnumerable<Claim>> GetValidClaimsAsync(AppUser user)
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
                new Claim(JwtUserIdClaimName, user.Id.ToString()),
            };
            var userClaims = await userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await roleManager.FindByNameAsync(userRole);
                if (role == null)
                {
                    continue;
                }

                var roleClaims = await roleManager.GetClaimsAsync(role);
                claims.AddRange(roleClaims);
            }

            return claims;
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var modifiedTokenValidationParameters = this.tokenValidationParameters.Clone();
                modifiedTokenValidationParameters.ValidateLifetime = false;
                var principal =
                    tokenHandler.ValidateToken(token, modifiedTokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }
    }
}