using System.Threading.Tasks;
using Isitar.DoenerOrder.Contracts.Requests;
using Isitar.DoenerOrder.Domain.Responses;

namespace Isitar.DoenerOrder.Services
{
    public interface IIdentityService
    {
        Task<AuthResponse> LoginAsync(LoginViewModel loginViewModel);
        Task<bool> RegisterAsync(RegistrationViewModel registrationViewModel);
    }
}