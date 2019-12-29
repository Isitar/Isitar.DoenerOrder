using System;

namespace Isitar.DoenerOrder.Options
{
    public class JwtSettings
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Secret { get; set; }

        public TimeSpan TokenLifetime { get; set; }
    }
}