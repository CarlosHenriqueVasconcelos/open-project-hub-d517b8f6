using System.Security.Claims;
using System.Text.Json.Serialization;
using PlataformaGestaoIA.Controllers.Functions;

namespace PlataformaGestaoIA.Models;

public class User
{
    private string? _slug;

    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    [JsonIgnore]
    public string? PasswordHash { get; set; }
    public string? Image { get; set; }
    public string? Slug
    {
        get => _slug;
        set => _slug = string.IsNullOrEmpty(value) ? GeneralFunction.GenerateTag(Email) : value;
    }
    public string? Bio { get; set; }

    public IList<Role>? Roles { get; set; }
    public IList<Project>? Projects { get; set; }
}