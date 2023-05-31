using Data;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Dto.User;

namespace Server.Services;

public class UserService
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _db;

    public UserService(UserManager<User> userManager, ApplicationDbContext db)
    {
        _userManager = userManager;
        _db = db;
    }

    public async Task<UserInfoDto> GetUserInfoAsync(User user)
    {
        var dbUser = await _db.Users
            .Include(x => x.Company)
            .FirstAsync(x => x.Id == user.Id);

        return new UserInfoDto
        {
            Username = dbUser.UserName,
            CompanyName = dbUser.Company.Name
        };
    }
}