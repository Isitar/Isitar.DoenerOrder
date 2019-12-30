namespace Isitar.DoenerOrder.Contracts.Requests
{
    public class RefreshTokenViewModel
    {
        public string RefreshToken { get; set; }
        public string JwtToken { get; set; }
    }
}