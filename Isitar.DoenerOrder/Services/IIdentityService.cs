using System.Threading.Tasks;
using Isitar.DoenerOrder.Contracts.Requests;
using Isitar.DoenerOrder.Domain.Responses;

namespace Isitar.DoenerOrder.Services
{
    public interface IIdentityService
    {
        Task<AuthResponse> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(string username, string email, string password);
        Task<AuthResponse> RefreshAsync(string refreshToken, string jwtToken);
    }
}