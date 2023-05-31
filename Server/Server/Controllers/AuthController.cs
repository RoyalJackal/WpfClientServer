using Data;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Dto.Auth;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly TokenService _tokenService;
    private readonly ApplicationDbContext _db;

    public AuthController(UserManager<User> userManager, TokenService tokenService, ApplicationDbContext db)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _db = db;
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<TokenDto>> SignIn([FromBody] SignInDto dto)
    {

        var user = await _userManager.FindByNameAsync(dto.Username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password)) return Unauthorized();

        var (accessToken, expiration) = _tokenService.CreateToken(user);
        return Ok(new TokenDto(accessToken, expiration));
    }

    [HttpPost]
    [Route("register")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TokenDto>> SignUp([FromBody] SignUpDto dto)
    {
        var userExists = await _userManager.FindByNameAsync(dto.Username);
        if (userExists != null)
            return BadRequest("Username occupied.");

        var company = await _db.Companies.FirstOrDefaultAsync(x => x.Id == dto.CompanyId);
        if (company == null)
            return BadRequest("Company does not exist.");

        var user = new User {UserName = dto.Username, Email = dto.Email, Company = company};
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded) return BadRequest("Undocumented error.");

        var (accessToken, expiration) = _tokenService.CreateToken(user);
        return Ok(new TokenDto(accessToken, expiration));
    }
}