using System.Security.Claims;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Api.Helpers.Auth;
using Isitar.DoenerOrder.Auth.Data.DAO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Isitar.DoenerOrder.Api.Controllers.V1
{
    public class UserController : ApiController
    {
        private readonly UserManager<AppUser> userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
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