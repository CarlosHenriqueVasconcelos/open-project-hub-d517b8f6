using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.DataContext;
using PlataformaGestaoIA.Models;

namespace PlataformaGestaoIA.Controllers.Functions
{
    public class UserControllerFunction
    {
        public async static Task<User> GetExisting(User user, PrincipalDataContext context)
        {
            var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);

            if (existingUser != null)
            {
                return existingUser;
            }

            return user;
        }

        public static async Task AddUserRoleAsync(User user, string roleName, PrincipalDataContext context)
        {
            if (user.Roles == null)
            {
                user.Roles = new List<Role>();
            }

            var role = await RoleControllerFunction.ReturnRole(roleName, context);

            if (!user.Roles.Any(r => r.Slug == role.Slug))
            {
                user.Roles.Add(role);

                if (role.Id == 0)
                {
                    context.Role.Add(role);
                }

                await context.SaveChangesAsync();
            }
        }
    }


}