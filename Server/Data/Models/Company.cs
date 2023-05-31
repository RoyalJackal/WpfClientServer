namespace Data.Models;

public class Company
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public List<User> Employees { get; set; }
}