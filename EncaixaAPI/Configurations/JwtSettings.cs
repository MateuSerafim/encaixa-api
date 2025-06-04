namespace EncaixaAPI.Configurations;
public record JwtSettings(string SecretKey = "",
                          int ExpiryMinutes = 5,
                          string Issuer = "",
                          string Audience = "")
{
}
