using Microsoft.AspNetCore.Mvc;
using Server.Dto.Company;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class CompanyController : ControllerBase
{
    private readonly CompanyService _companyService;

    public CompanyController(CompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet]
    [Route("")]
    [ProducesResponseType(typeof(List<CompanyListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CompanyListDto>>> GetCompanies() =>
        Ok(await _companyService.GetCompaniesAsync());
    
    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<CompanyListDto>>> GetEmployees(int id)
    {
        var (employees, error) = await _companyService.GetEmployeesAsync(id);
        if (error != null)
            return BadRequest(error);

        return Ok(employees);
    }
}