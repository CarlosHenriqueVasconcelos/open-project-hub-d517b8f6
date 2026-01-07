using PlataformaGestaoIA.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PlataformaGestaoIA.Models;

public class Project
{
    public int Id { get; set; }

    public string Description { get; set; }

    public string InternalCode { get; set; }

    public IList<ProjectSkill>? ProjectSkill { get; set; }

    public ICollection<User>? Users { get; set; }
}

