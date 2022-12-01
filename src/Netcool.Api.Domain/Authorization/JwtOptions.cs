namespace Netcool.Api.Domain.Authorization;

public class JwtOptions
{
    public bool ValidateIssuer { get; set; }

    public bool ValidateAudience { get; set; }

    public bool ValidateLifetime { get; set; }

    public bool ValidateIssuerSigningKey { get; set; }

    public string IssuerSigningKey { get; set; }

    public string ValidAudience { get; set; }

    public string ValidIssuer { get; set; }

    public int ExpiryMinutes { get; set; } = 120;
}
