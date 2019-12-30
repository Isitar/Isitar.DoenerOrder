using System;

namespace Isitar.DoenerOrder.Domain.DAO
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
        public User User { get; set; }
    }
}