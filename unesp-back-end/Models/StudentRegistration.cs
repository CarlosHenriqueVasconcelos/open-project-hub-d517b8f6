namespace PlataformaGestaoIA.Models
{
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    public class StudentRegistration
    {
        [JsonIgnore]
        public int Id { get; set; }
        public Student? Student { get; set; }
        public string? SkillsDescription { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public short? Subject { get; set; } // 0, 1, or 2
        public short? ChoicePriority { get; set; } // 1 or 2
        public bool? EnrolledInIndustry4_0 { get; set; }
        public short? DoesNotMeetRequirements { get; set; } // 1 or 2
        [JsonPropertyName("cursarUmaOuDuas")]
         public bool? CursarUmaOuDuas { get; set; }  
        public bool? Presencial { get; set; } // 1 or 2
        public DateTime? FirstMeetingDate { get; set; }
        public string? Semester { get; set; }
        [JsonPropertyName("studentRegistrationScore")]
        public StudentRegistrationScore? StudentRegistrationScore { get; set; }
        [JsonPropertyName("studentSkills")]
        public IList<StudentRegistrationSkill>? StudentSkills { get; set; } = new List<StudentRegistrationSkill>(); // Coleção de skills
        public string? FilePath { get; set; }

        // Override ToString method for better output readability
        public override string ToString()
        {
            var skillNames = StudentSkills.Select(skill => skill.Skill.Name);

            return $"Registration ID: {Id}, Student ID: {Student.Id}, Skills: {SkillsDescription}, Registration Date: {RegistrationDate?.ToShortDateString() ?? "Data não informada"}, Semester: {Semester}, Subject: {Subject}, Priority: {ChoicePriority}, Industry 4.0: {EnrolledInIndustry4_0}, Requirements Not Met: {DoesNotMeetRequirements}, First Meeting: {FirstMeetingDate?.ToShortDateString() ?? "Data não informada"}, " +
                $"Performance Coefficient: {StudentRegistrationScore.PerformanceCoefficient}, Scientific Initiation Score: {StudentRegistrationScore.ScientificInitiationProgramScore}, Institutional Monitoring Score: {StudentRegistrationScore.InstitutionalMonitoringProgramScore}, " +
                $"Junior Enterprise Experience Score: {StudentRegistrationScore.JuniorEnterpriseExperienceScore}, Project in Technological Hotel Score: {StudentRegistrationScore.ProjectInTechnologicalHotelScore}, Volunteering Score: {StudentRegistrationScore.VolunteeringScore}, " +
                $"High Grade Discipline Score: {StudentRegistrationScore.HighGradeDisciplineScore}, Certification Courses Score: {StudentRegistrationScore.CertificationCoursesScore}, High Grade Courses Score: {StudentRegistrationScore.HighGradeCoursesScore}, AI Projects Score: {StudentRegistrationScore.AIProjectsScore}, " +
                $"Skills: [{string.Join(", ", skillNames)}]";
        }
    }
}
