using System.Security.Claims;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Api.Helpers.Auth;
using Isitar.DoenerOrder.Api.Core.Data;
using Isitar.DoenerOrder.Api.Core.Domain.DAO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Isitar.DoenerOrder.Api.Controllers.V1
{
    public class UserController : ApiController
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly DoenerOrderContext dbContext;

        public UserController(IConfiguration configuration,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            DoenerOrderContext dbContext)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.dbContext = dbContext;
        }
        

        [HttpGet("setup")]
        public async Task<IActionResult> SetupClaims()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var res = await userManager.AddClaimAsync(user,
                new Claim(CustomClaimTypes.Permission, ClaimPermission.CreateBulkOrder));
            if (!res.Succeeded)
            {
                return BadRequest(res.Errors);
            }
            return Ok();
        }

      
    }
}