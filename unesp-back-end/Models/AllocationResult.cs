using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PlataformaGestaoIA.Models;

namespace PlataformaGestaoIA.Models
{
    public class AllocationResult
    {
        [JsonIgnore]
        public int Id { get; set; }

        public Student Student { get; set; }

        public Project Project { get; set; }

        [StringLength(7)]
        public string Semester { get; set; }
    }
}