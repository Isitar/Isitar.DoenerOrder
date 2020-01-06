using System.Threading.Tasks;
using Isitar.DoenerOrder.Auth.Responses;

namespace Isitar.DoenerOrder.Auth.Services
{
    public interface IIdentityService
    {
        Task<AuthResponse> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(string username, string email, string password);
        Task<AuthResponse> RefreshAsync(string refreshToken, string jwtToken);
    }
}