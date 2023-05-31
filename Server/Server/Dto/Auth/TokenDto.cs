namespace Server.Dto.Auth;

public class TokenDto
{
    public TokenDto(string token, DateTime validUntil)
    {
        Token = token;
        ValidUntil = validUntil;
    }

    public string Token { get; set; }
    
    public DateTime ValidUntil { get; set; }
}