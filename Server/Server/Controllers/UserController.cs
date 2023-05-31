using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Dto.User;
using Server.Services;

namespace Server.Controllers;

public class UserController : AuthorizedControllerBase
{
    private readonly UserService _userService;
    
    public UserController(UserManager<User> userManager, UserService userService) : base(userManager)
    {
        _userService = userService;
    }
    
    [HttpGet]
    [Authorize]
    [Route("info")]
    [ProducesResponseType(typeof(UserInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<UserInfoDto>>> CurrentUserInfo()
    {
        var (user, authError) = await CheckAuth();
        if (user == null)
            return BadRequest(authError);

        var userInfo = await _userService.GetUserInfoAsync(user);

        return Ok(userInfo);
    }
}