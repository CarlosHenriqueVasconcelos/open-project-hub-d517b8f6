using System.Security.Claims;
using PlataformaGestaoIA.Models;

namespace PlataformaGestaoIA.Extensions
{
	public static class RoleClaimExtention
	{
		public static IEnumerable<Claim> GetClaims(this User user)
		{
			var result = new List<Claim>()
			{
				new Claim(ClaimTypes.Name, user.Email)
			};

			result.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));

			return result;
		}
	}
}
