namespace Netcool.Api.Domain.Authorization
{
    public class JwtOptions
    {
        public string Secret { get; set; }

        public string Audience { get; set; }

        public string Issuer { get; set; }

        public int ExpiryMinutes { get; set; }
    }
}