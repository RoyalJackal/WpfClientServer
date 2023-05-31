namespace Client.Dto;

public class CompanyDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }

    public override string ToString() => Name;
}