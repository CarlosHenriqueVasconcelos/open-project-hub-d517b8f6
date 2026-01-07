using System.Text.Json.Serialization;

namespace PlataformaGestaoIA.Models
{
    public class StudentRegistrationScore
    {
        [JsonIgnore]
        public int Id { get; set; }
        public float? PerformanceCoefficient { get; set; }
        public int? ScientificInitiationProgramScore { get; set; }
        public int? InstitutionalMonitoringProgramScore { get; set; }
        public int? JuniorEnterpriseExperienceScore { get; set; }
        public int? ProjectInTechnologicalHotelScore { get; set; }
        public int? VolunteeringScore { get; set; }
        public int? HighGradeDisciplineScore { get; set; }
        public int? CertificationCoursesScore { get; set; }
        public int? HighGradeCoursesScore { get; set; }
        public int? AIProjectsScore { get; set; }
        public int? InternshipEmploymentScore { get; set; }
        public int? TechnologyCertificationScore { get; set; }
        public int? LowLevelTechScore { get; set; }
        public string? ScoreCoursesDescription { get; set; }
    }
}
