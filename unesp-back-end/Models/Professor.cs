using System.Text.Json.Serialization;

namespace PlataformaGestaoIA.Models;

public class Professor
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Name { get; set; }
    public User? User { get; set; }
}