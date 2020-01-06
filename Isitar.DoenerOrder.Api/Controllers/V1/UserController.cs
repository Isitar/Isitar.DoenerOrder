using System.Security.Claims;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Api.Helpers.Auth;
using Isitar.DoenerOrder.Auth.Data.DAO;
using Isitar.DoenerOrder.Core.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Isitar.DoenerOrder.Api.Controllers.V1
{
    public class UserController : ApiController
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly DoenerOrderContext dbContext;

        public UserController(IConfiguration configuration,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
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