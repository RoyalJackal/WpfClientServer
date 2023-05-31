using Data;
using Microsoft.EntityFrameworkCore;
using Server.Dto.Company;

namespace Server.Services;

public class CompanyService
{
    private readonly ApplicationDbContext _db;

    public CompanyService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<CompanyListDto>> GetCompaniesAsync() =>
        await _db.Companies
            .Select(x => new CompanyListDto(x.Id, x.Name))
            .ToListAsync();

    public async Task<(List<string>?, string?)> GetEmployeesAsync(int id)
    {
        var company = await _db.Companies
            .Include(x => x.Employees)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (company == null)
            return (null, "No company with this id found");

        return (company.Employees
            .Select(x => x.UserName)
            .ToList(), null);
    }
}