using System.Text.Json.Serialization;

namespace PlataformaGestaoIA.Models
{
    public class StudentRegistrationSkill
    {
        [JsonIgnore]
        public int Id { get; set; }
        public Skill? Skill { get; set; }
        public short? Level { get; set; }
        [JsonIgnore]
        public IList<StudentRegistration>? StudentRegistration { get; set; } = new List<StudentRegistration>(); // Coleção de skills
    }

    public class TempStudentRegistrationSkill
    {
        public TempSkill Skill { get; set; }
        public short Level { get; set; }
    }
}
