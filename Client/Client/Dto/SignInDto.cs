using System.Text.Json.Serialization;

namespace Client.Dto;

public class SignInDto
{
    public SignInDto(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public string Username { get; set; }
    
    public string Password { get; set; }
}