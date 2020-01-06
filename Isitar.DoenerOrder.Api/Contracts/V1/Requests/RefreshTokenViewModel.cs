namespace Isitar.DoenerOrder.Api.Contracts.V1.Requests
{
    public class RefreshTokenViewModel
    {
        public string RefreshToken { get; set; }
        public string JwtToken { get; set; }
    }
}