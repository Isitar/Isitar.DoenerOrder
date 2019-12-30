using System.Threading.Tasks;
using Isitar.DoenerOrder.Contracts.Requests;
using Isitar.DoenerOrder.Contracts.Responses;
using Isitar.DoenerOrder.Contracts.V1;
using Isitar.DoenerOrder.Domain.Responses;
using Isitar.DoenerOrder.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            var res = await identityService.RegisterAsync(registrationViewModel.Username, registrationViewModel.Email, registrationViewModel.Password);
            return res ? (IActionResult) Ok() : BadRequest();
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Auth.Refresh)]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AuthFailedResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenViewModel refreshTokenViewModel)
        {
            var resp = await identityService.RefreshAsync(refreshTokenViewModel.RefreshToken, refreshTokenViewModel.JwtToken);
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
        
        [HttpPost(ApiRoutes.Auth.Login)]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AuthFailedResponse), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetTokenAsync([FromBody] LoginViewModel loginViewModel)
        {
            var resp = await identityService.LoginAsync(loginViewModel.Username, loginViewModel.Password);
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