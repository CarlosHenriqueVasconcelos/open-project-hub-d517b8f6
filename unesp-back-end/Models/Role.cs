using System.Text.Json.Serialization;

namespace PlataformaGestaoIA.Models;

public class Role
{
    public int Id { get; set; }
	public string Name { get; set; }
	public string Slug { get; set; }
	[JsonIgnore]
	public IList<User>? Users { get; set; }
}
