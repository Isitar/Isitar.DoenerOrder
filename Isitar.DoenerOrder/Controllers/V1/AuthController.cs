using System.Threading.Tasks;
using Isitar.DoenerOrder.Contracts.Requests;
using Isitar.DoenerOrder.Contracts.Responses;
using Isitar.DoenerOrder.Contracts.V1;
using Isitar.DoenerOrder.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Isitar.DoenerOrder.Controllers
{
    public class AuthController : ApiController
    {
        private readonly IIdentityService identityService;

        public AuthController(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        [HttpPost(ApiRoutes.Auth.Register)]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationViewModel registrationViewModel)
        {
            var res = await identityService.RegisterAsync(registrationViewModel);
            return res ? (IActionResult) Ok() : BadRequest();
        }

        [HttpPost(ApiRoutes.Auth.Login)]
        [AllowAnonymous]
        public async Task<IActionResult> GetTokenAsync([FromBody] LoginViewModel loginViewModel)
        {
            var resp = await identityService.LoginAsync(loginViewModel);
            if (!resp.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Message = resp.Message
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = resp.Token,
                RefreshToken = resp.RefreshToken,
            });
        }
    }
}