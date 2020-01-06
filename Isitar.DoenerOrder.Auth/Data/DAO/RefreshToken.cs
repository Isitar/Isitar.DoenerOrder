using System;

namespace Isitar.DoenerOrder.Auth.Data.DAO
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string JwtTokenId { get; set; }
        public DateTime Expires { get; set; }
        public bool Used { get; set; }
        public bool Invalidated { get; set; }
        
        public int UserId { get; set; }
        public AppUser User { get; set; }
    }
}