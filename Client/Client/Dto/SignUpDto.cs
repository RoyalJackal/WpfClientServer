namespace Client.Dto;

public class SignUpDto
{
    public SignUpDto(string username, string email, string password, int companyId)
    {
        Username = username;
        Email = email;
        Password = password;
        CompanyId = companyId;
    }

    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public int CompanyId { get; set; }
}