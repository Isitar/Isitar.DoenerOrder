namespace Isitar.DoenerOrder.Api.Contracts.V1.Responses
{
    public class AuthSuccessResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}