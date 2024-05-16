using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.CoreLayer.Entities.Idintity;

namespace Talabat.APIs.Extentions
{
    public static class UserMangerExtensions
    {
        public static async Task<ApplicationUser> FindUserWithAddressAsync(this UserManager<ApplicationUser> userManager, ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

            var user = await userManager.Users.Include(u => u.Adress).FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpper());

            return user;
        }
    }
}
