using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace PlataformaGestaoIA.Models;

public class Skill
{
    public int Id { get; set; }
    public string? Name { get; set; }
    [ValidateNever]
    public string? Tag { get; set; }
    public bool? IsSoftSkill { get; set; }
}

public class TempSkill
{
    public string? Name { get; set; }
    public string? Tag { get; set; }
}
