using System;

namespace PlataformaGestaoIA.Models
{
    public class StudentRegistrationRanking
    {
        public int Id { get; set; }
        public int StudentRegistrationId { get; set; }
        public StudentRegistration? StudentRegistration { get; set; }
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        public string Semester { get; set; }
        public short SubjectValue { get; set; }
        public float TotalScore { get; set; }
        public float PerformanceCoefficient { get; set; }
        public int RankPosition { get; set; }
        public string Classification { get; set; }
        public string Status { get; set; }
        public DateTime? ConfirmBy { get; set; }
        public DateTime? StatusUpdatedAt { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
