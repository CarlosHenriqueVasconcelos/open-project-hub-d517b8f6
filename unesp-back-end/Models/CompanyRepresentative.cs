using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PlataformaGestaoIA.Models
{
    public class CompanyRepresentative
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; }

        public string InternalCode { get; set; }

        public string CPF { get; set; }

        public Company? Company { get; set; }

        public User? User { get; set; }
    }
}
