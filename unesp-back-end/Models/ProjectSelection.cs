using System.Text.Json.Serialization;

namespace PlataformaGestaoIA.Models
{
    public class ProjectSelection
    {
        [JsonIgnore]
        public int Id { get; set; }
        public Student Student { get; set; }
        public Project Project { get; set; }
        public string Semester { get; set; } 
    }
}
