using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class UserManagerExtensions
    {

        public static async Task<AppUser> FindByEmailFromClaimsPrincipalWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            string email = user.FindFirstValue(ClaimTypes.Email);
            return await userManager.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Email == email);
        }

        public static async Task<AppUser> FindByEmailFromClaimsPrincipalAsync(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            string email = user.FindFirstValue(ClaimTypes.Email);
            return await userManager.Users.SingleOrDefaultAsync(x => x.Email == email);
        }
    }
}