using System;

namespace PlataformaGestaoIA.ViewModel
{
    public class RankingGenerateViewModel
    {
        public string? Semester { get; set; }
    }

    public class RankingStatusUpdateViewModel
    {
        public string Email { get; set; }
        public string Status { get; set; }
    }

    public class RankingEntryViewModel
    {
        public int Id { get; set; }
        public int StudentRegistrationId { get; set; }
        public string Semester { get; set; }
        public short SubjectValue { get; set; }
        public int RankPosition { get; set; }
        public string Classification { get; set; }
        public string Status { get; set; }
        public float TotalScore { get; set; }
        public float PerformanceCoefficient { get; set; }
        public DateTime? ConfirmBy { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public RankingStudentViewModel? Student { get; set; }
    }

    public class RankingStudentViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? RA { get; set; }
        public string? CPF { get; set; }
        public string? RG { get; set; }
        public string? Cellphone { get; set; }
        public string? Email { get; set; }
        public RankingCourseViewModel? CurrentCourse { get; set; }
    }

    public class RankingCourseViewModel
    {
        public string? Description { get; set; }
        public string? Mode { get; set; }
        public string? Period { get; set; }
        public string? Campus { get; set; }
    }
}
