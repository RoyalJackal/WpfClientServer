namespace Server.Dto.Company;

public class CompanyListDto
{
    public CompanyListDto(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; set; }
    
    public string Name { get; set; }
}