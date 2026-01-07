using PlataformaGestaoIA.Models;
using System.Text.Json;

namespace PlataformaGestaoIA.Controllers.Functions
{
    public class StudentRegistrationScoreControllerFunction
    {
        public static void Update(StudentRegistrationScore oldStudentRegistrationScore, StudentRegistrationScore newStudentRegistrationScore)
        {
            oldStudentRegistrationScore.PerformanceCoefficient = newStudentRegistrationScore.PerformanceCoefficient;
            oldStudentRegistrationScore.ScientificInitiationProgramScore = newStudentRegistrationScore.ScientificInitiationProgramScore;
            oldStudentRegistrationScore.InstitutionalMonitoringProgramScore = newStudentRegistrationScore.InstitutionalMonitoringProgramScore;
            oldStudentRegistrationScore.JuniorEnterpriseExperienceScore = newStudentRegistrationScore.JuniorEnterpriseExperienceScore;
            oldStudentRegistrationScore.ProjectInTechnologicalHotelScore = newStudentRegistrationScore.ProjectInTechnologicalHotelScore;
            oldStudentRegistrationScore.VolunteeringScore = newStudentRegistrationScore.VolunteeringScore;
            oldStudentRegistrationScore.HighGradeDisciplineScore = newStudentRegistrationScore.HighGradeDisciplineScore;
            oldStudentRegistrationScore.CertificationCoursesScore = newStudentRegistrationScore.CertificationCoursesScore;
            oldStudentRegistrationScore.HighGradeCoursesScore = newStudentRegistrationScore.HighGradeCoursesScore;
            oldStudentRegistrationScore.AIProjectsScore = newStudentRegistrationScore.AIProjectsScore;
            oldStudentRegistrationScore.InternshipEmploymentScore = newStudentRegistrationScore.InternshipEmploymentScore;
            oldStudentRegistrationScore.TechnologyCertificationScore = newStudentRegistrationScore.TechnologyCertificationScore;
            oldStudentRegistrationScore.LowLevelTechScore = newStudentRegistrationScore.LowLevelTechScore;
            oldStudentRegistrationScore.ScoreCoursesDescription = newStudentRegistrationScore.ScoreCoursesDescription;
        }

        public static StudentRegistrationScore DeserializeStudentScore(string studentScoreJson)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Ignora maiúsculas/minúsculas nas propriedades
                };

                var tempScore = JsonSerializer.Deserialize<StudentRegistrationScore>(studentScoreJson, options);

                if (tempScore == null)
                {
                    throw new Exception("Falha ao desserializar studentSkills: resultado é nulo.");
                }

                return tempScore;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao desserializar studentSkills: {ex.Message}");
            }
        }
    }
}
