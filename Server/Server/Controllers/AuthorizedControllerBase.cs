using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

public class AuthorizedControllerBase : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public AuthorizedControllerBase(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<(User?, string?)> CheckAuth()
    {
        var username = User.Identity?.Name;
        if (username == null)
            return (null, "User not found.");

        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
            return (null, "User not found.");

        return (user, null);
    }
}