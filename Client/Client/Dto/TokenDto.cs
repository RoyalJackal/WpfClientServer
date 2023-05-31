using System;

namespace Client.Dto;

public class TokenDto
{
    public string Token { get; set; }
    
    public DateTime ValidUntil { get; set; }
}