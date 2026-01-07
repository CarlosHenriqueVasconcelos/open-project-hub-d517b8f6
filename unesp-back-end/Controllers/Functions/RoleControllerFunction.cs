using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.Controllers.Functions;
using PlataformaGestaoIA.DataContext;
using PlataformaGestaoIA.Models;

namespace PlataformaGestaoIA.Controllers.Functions
{
    public class RoleControllerFunction
    {
        public static async Task<Role> ReturnRole(string name, PrincipalDataContext context)
        {
            var role = context.Role.AsTracking().FirstOrDefault(x => x.Slug == GeneralFunction.GenerateTag(name));

            if (role != null)
            {
                return role;
            }
            
            return new Role()
            {
                Name = name,
                Slug = GeneralFunction.GenerateTag(name)
            };
        }
    }
}
