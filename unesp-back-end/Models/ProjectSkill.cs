using System.Text.Json.Serialization;

namespace PlataformaGestaoIA.Models
{
    public class ProjectSkill
    {
        public int Id { get; set; }
        public Skill Skill { get; set; }
        public int Level { get; set; } 
        public IList<Project>? Project { get; set; }

    }
}
