using PlataformaGestaoIA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.DataContext;
using PlataformaGestaoIA.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlataformaGestaoIA.ViewModel;
using PlataformaGestaoIA.Services;
using SecureIdentity.Password;
using PlataformaGestaoIA.Attributes;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using PlataformaGestaoIA.Controllers.Functions;
using System.Data;

namespace PlataformaGestaoIA.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
	[HttpPost("api/v1/accounts/")]
	[ProducesResponseType(typeof(ResponseRegisterViewModel), 200)]
	[ProducesResponseType(typeof(ResultViewModel<string>), 400)]
	[ProducesResponseType(typeof(ResultViewModel<string>), 500)]
	public async Task<IActionResult> Post(
		[FromBody] RegisterViewModel model,
		[FromServices] PrincipalDataContext context)
	{
        context.Database.Migrate();

        if (!ModelState.IsValid)
			return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

		var user = new User
		{
			Name = model.Name,
			Email = model.Email,
			Slug = GeneralFunction.GenerateTag(model.Email)
        };

        await UserControllerFunction.AddUserRoleAsync(user, "Professor", context);

        var password = PasswordGenerator.Generate(25);
		user.PasswordHash = PasswordHasher.Hash(password);

		try
		{
			await context.Users.AddAsync(user);
			await context.SaveChangesAsync();

			return Ok(new ResultViewModel<dynamic>(new
			{
				user = user.Email,
				password
			}));
		}
		catch (DbUpdateException)
		{
			return StatusCode(400, new ResultViewModel<string>("05X99 - Este E-mail já está cadastrado"));
		}
		catch
		{
			return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
		}
	}

	[HttpPost("api/v1/accounts/login")]
	[ProducesResponseType(typeof(ResponseLoginViewModel), 200)]
	[ProducesResponseType(typeof(ResultViewModel<string>), 401)]
	[ProducesResponseType(typeof(ResultViewModel<string>), 500)]
	public async Task<IActionResult> Login(
		[FromBody] LoginViewModel model,
		[FromServices] PrincipalDataContext context,
		[FromServices] TokenService tokenService)
	{
        context.Database.Migrate();

        if (!ModelState.IsValid)
			return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

		var user = await context
			.Users
			.AsNoTracking()
			.Include(x => x.Roles)
			.FirstOrDefaultAsync(x => x.Email == model.Email);

		if (user == null)
			return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));

		if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
			return StatusCode(401, new ResultViewModel<string>($"Usuário ou senha inválidos, {user.PasswordHash} != {model.Password}"));

		try
		{
			var token = tokenService.GenerateToken(user);
			return Ok(new ResponseLoginViewModel() { Token = token});
		}
		catch
		{
			return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
		}
	}

	[Authorize]
	[ProducesResponseType(typeof(ResponseRegisterViewModel), 200)]
	[ProducesResponseType(typeof(ResultViewModel<string>), 500)]
	[HttpPost("api/v1/accounts/upload-image")]
	public async Task<IActionResult> UploadImage(
		[FromBody] UploadImageViewModel model,
		[FromServices] PrincipalDataContext context)
	{
		var fileName = $"{Guid.NewGuid().ToString()}.jpg";
		var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(model.Base64Image, "");
		var bytes = Convert.FromBase64String(data);

		try
		{
			await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
		}

		var user = await context
			.Users
			.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

		if (user == null)
			return NotFound(new ResultViewModel<User>("Usuário não encontrado"));

		user.Image = $"https://localhost:0000/images/{fileName}";
		try
		{
			context.Users.Update(user);
			await context.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
		}

		return Ok(new ResultViewModel<string>("Imagem alterada com sucesso!", null));
	}

    [ProducesResponseType(typeof(ResultViewModel<User>), 200)]
    [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
    [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
    [HttpGet("api/v1/users")]
    public async Task<IActionResult> GetAllAsync(
                [FromServices] PrincipalDataContext context)
    {
        try
        {
            var usersWithRoles = await context.Users
			.Include(u => u.Roles) // Carrega as roles relacionadas
			.Select(user => new UserViewModel
			{
				Name = user.Name,
				Email = user.Email,
				Roles = string.Join(", ", user.Roles.Select(role => role.Name))
			})
			.ToListAsync();
            return Ok(new ResultViewModel<List<UserViewModel>>(usersWithRoles));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<User>>("05X04 - Falha interna no servidor"));
        }
    }
}