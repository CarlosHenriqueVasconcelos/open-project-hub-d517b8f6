using System.Text.Json.Serialization;

namespace PlataformaGestaoIA.Models;

public class Company
{
    [JsonIgnore]
    public int Id { get; set; }

    public string Name { get; set; }
    public string LegalName { get; set; }
    public string CNPJ { get; set; }
}
