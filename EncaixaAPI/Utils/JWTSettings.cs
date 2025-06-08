namespace EncaixaAPI.Utils;
public record JWTSettings(
    string SecretKey,
    int ExpiryMinutes = 10,
    string Issuer = "EncaixaAPI",
    string Audience = "Clients")
{
}
