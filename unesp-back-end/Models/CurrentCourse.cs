using System.Text.Json.Serialization;

namespace PlataformaGestaoIA.Models
{
    public class CurrentCourse
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Mode { get; set; }
        public string Description { get; set; }
        public string Period { get; set; }
        public string Campus { get; set; }
    }
}
