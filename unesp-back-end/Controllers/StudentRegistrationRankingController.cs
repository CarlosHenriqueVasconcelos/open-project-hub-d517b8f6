using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.DataContext;
using PlataformaGestaoIA.Models;
using PlataformaGestaoIA.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaGestaoIA.Controllers
{
    [ApiController]
    public class StudentRegistrationRankingController : ControllerBase
    {
        private const string ClassificationClassificado = "classificado";
        private const string ClassificationEspera = "espera";
        private const string StatusPendente = "pendente";
        private const string StatusConfirmado = "confirmado";
        private const string StatusDesistencia = "desistencia";
        private const string StatusExpirado = "expirado";
        private const string StatusEspera = "espera";

        private static readonly short[] SubjectValues = { 1, 2, 4, 8, 16, 32 };
        private static readonly Dictionary<short, short> PriorityToSubject = new()
        {
            { 1, 1 },
            { 2, 4 },
            { 3, 2 },
            { 4, 8 },
            { 5, 16 },
            { 6, 32 }
        };

        [Authorize]
        [HttpPost("api/v1/rankings/generate")]
        public async Task<IActionResult> Generate(
            [FromBody] RankingGenerateViewModel model,
            [FromServices] PrincipalDataContext context)
        {
            var semester = string.IsNullOrWhiteSpace(model?.Semester)
                ? CalculateSemester(DateTime.Now)
                : model.Semester;

            var config = await context.GeneralConfigs.FirstOrDefaultAsync();
            var confirmBy = config?.ConfirmationDeadline;
            var now = DateTime.UtcNow;

            var existing = context.StudentRegistrationRankings
                .Where(r => r.Semester == semester);
            context.StudentRegistrationRankings.RemoveRange(existing);

            var registrations = await context.StudentRegistrations
                .Include(sr => sr.Student)
                    .ThenInclude(s => s.User)
                .Include(sr => sr.StudentRegistrationScore)
                .Where(sr => sr.Semester == semester)
                .ToListAsync();

            var entries = new List<StudentRegistrationRanking>();

            foreach (var registration in registrations)
            {
                if (registration.Student == null)
                {
                    continue;
                }

                var subjects = GetSubjects(registration.Subject);
                if (subjects.Count == 0)
                {
                    continue;
                }

                if (subjects.Count > 1 && registration.CursarUmaOuDuas != true)
                {
                    var prioritySubject = GetPrioritySubject(registration.ChoicePriority);
                    if (prioritySubject.HasValue && subjects.Contains(prioritySubject.Value))
                    {
                        subjects = new List<short> { prioritySubject.Value };
                    }
                    else
                    {
                        subjects = new List<short> { subjects[0] };
                    }
                }

                var totalScore = CalculateTotalScore(registration.StudentRegistrationScore);
                var performanceCoefficient = registration.StudentRegistrationScore?.PerformanceCoefficient ?? 0f;
                var registrationDate = registration.RegistrationDate;

                foreach (var subject in subjects)
                {
                    entries.Add(new StudentRegistrationRanking
                    {
                        StudentRegistrationId = registration.Id,
                        StudentId = registration.Student.Id,
                        Semester = registration.Semester,
                        SubjectValue = subject,
                        TotalScore = totalScore,
                        PerformanceCoefficient = performanceCoefficient,
                        RankPosition = 0,
                        Classification = ClassificationEspera,
                        Status = StatusEspera,
                        ConfirmBy = confirmBy,
                        StatusUpdatedAt = now,
                        RegistrationDate = registrationDate,
                        CreatedAt = now,
                        UpdatedAt = now
                    });
                }
            }

            foreach (var subject in SubjectValues)
            {
                var subjectEntries = entries
                    .Where(e => e.SubjectValue == subject)
                    .OrderByDescending(e => e.TotalScore)
                    .ThenByDescending(e => e.PerformanceCoefficient)
                    .ThenBy(e => e.RegistrationDate ?? DateTime.MaxValue)
                    .ToList();

                for (var i = 0; i < subjectEntries.Count; i++)
                {
                    var entry = subjectEntries[i];
                    entry.RankPosition = i + 1;
                    if (i < 45)
                    {
                        entry.Classification = ClassificationClassificado;
                        entry.Status = StatusPendente;
                    }
                }
            }

            await context.StudentRegistrationRankings.AddRangeAsync(entries);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<string>($"Classificação gerada para {semester}. Registros: {entries.Count}"));
        }

        [HttpGet("api/v1/rankings")]
        public async Task<IActionResult> Get(
            [FromQuery] short subject,
            [FromQuery] string? semester,
            [FromServices] PrincipalDataContext context)
        {
            var currentSemester = string.IsNullOrWhiteSpace(semester)
                ? CalculateSemester(DateTime.Now)
                : semester;

            if (!SubjectValues.Contains(subject))
                return BadRequest(new ResultViewModel<string>("Matéria inválida."));

            await ApplyDeadlineAsync(context, currentSemester);

            var rankings = await context.StudentRegistrationRankings
                .Include(r => r.Student)
                    .ThenInclude(s => s.User)
                .Include(r => r.Student)
                    .ThenInclude(s => s.CurrentCourse)
                .Where(r => r.Semester == currentSemester && r.SubjectValue == subject)
                .OrderBy(r => r.RankPosition)
                .ToListAsync();

            var response = rankings.Select(MapRankingEntry).ToList();
            return Ok(new ResultViewModel<List<RankingEntryViewModel>>(response));
        }

        [HttpGet("api/v1/rankings/me")]
        public async Task<IActionResult> GetMine(
            [FromQuery] string email,
            [FromQuery] string? semester,
            [FromServices] PrincipalDataContext context)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(new ResultViewModel<string>("Email é obrigatório."));

            var currentSemester = string.IsNullOrWhiteSpace(semester)
                ? CalculateSemester(DateTime.Now)
                : semester;

            await ApplyDeadlineAsync(context, currentSemester);

            var rankings = await context.StudentRegistrationRankings
                .Include(r => r.Student)
                    .ThenInclude(s => s.User)
                .Include(r => r.Student)
                    .ThenInclude(s => s.CurrentCourse)
                .Where(r => r.Semester == currentSemester && r.Student.User.Email.ToLower() == email.ToLower())
                .OrderBy(r => r.SubjectValue)
                .ThenBy(r => r.RankPosition)
                .ToListAsync();

            var response = rankings.Select(MapRankingEntry).ToList();
            return Ok(new ResultViewModel<List<RankingEntryViewModel>>(response));
        }

        [HttpPost("api/v1/rankings/{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(
            [FromRoute] int id,
            [FromBody] RankingStatusUpdateViewModel model,
            [FromServices] PrincipalDataContext context)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Status))
                return BadRequest(new ResultViewModel<string>("Dados inválidos."));

            var status = model.Status.Trim().ToLowerInvariant();
            if (status != StatusConfirmado && status != StatusDesistencia)
                return BadRequest(new ResultViewModel<string>("Status inválido."));

            var ranking = await context.StudentRegistrationRankings
                .Include(r => r.Student)
                    .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (ranking == null)
                return NotFound(new ResultViewModel<string>("Registro não encontrado."));

            if (!string.Equals(ranking.Student?.User?.Email, model.Email, StringComparison.OrdinalIgnoreCase))
                return Forbid();

            if (!string.Equals(ranking.Classification, ClassificationClassificado, StringComparison.OrdinalIgnoreCase))
                return BadRequest(new ResultViewModel<string>("A confirmação só é permitida para classificados."));

            if (!string.Equals(ranking.Status, StatusPendente, StringComparison.OrdinalIgnoreCase))
                return BadRequest(new ResultViewModel<string>("Este registro não está pendente para confirmação."));

            if (ranking.ConfirmBy.HasValue && ranking.ConfirmBy.Value < DateTime.UtcNow)
            {
                ranking.Status = StatusExpirado;
                ranking.StatusUpdatedAt = DateTime.UtcNow;
                ranking.UpdatedAt = DateTime.UtcNow;
                await context.SaveChangesAsync();
                return BadRequest(new ResultViewModel<string>("Prazo de confirmação encerrado."));
            }

            ranking.Status = status;
            ranking.StatusUpdatedAt = DateTime.UtcNow;
            ranking.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            if (status == StatusDesistencia)
            {
                await PromoteNextAsync(context, ranking.Semester, ranking.SubjectValue, ranking.ConfirmBy);
            }

            return Ok(new ResultViewModel<RankingEntryViewModel>(MapRankingEntry(ranking)));
        }

        private static List<short> GetSubjects(short? subjectMask)
        {
            if (subjectMask == null)
                return new List<short>();

            return SubjectValues.Where(value => (subjectMask.Value & value) == value).ToList();
        }

        private static short? GetPrioritySubject(short? priority)
        {
            if (priority == null)
                return null;

            return PriorityToSubject.TryGetValue(priority.Value, out var subject) ? subject : null;
        }

        private static float CalculateTotalScore(StudentRegistrationScore? score)
        {
            if (score == null)
                return 0f;

            var total =
                (score.ScientificInitiationProgramScore ?? 0) +
                (score.InstitutionalMonitoringProgramScore ?? 0) +
                (score.JuniorEnterpriseExperienceScore ?? 0) +
                (score.ProjectInTechnologicalHotelScore ?? 0) +
                (score.VolunteeringScore ?? 0f) +
                (score.HighGradeDisciplineScore ?? 0) +
                (score.CertificationCoursesScore ?? 0) +
                (score.HighGradeCoursesScore ?? 0) +
                (score.AIProjectsScore ?? 0) +
                (score.InternshipEmploymentScore ?? 0) +
                (score.TechnologyCertificationScore ?? 0) +
                (score.LowLevelTechScore ?? 0);

            return (float)total;
        }

        private static string CalculateSemester(DateTime date)
        {
            var year = date.Year;
            var semester = date.Month <= 6 ? $"{year}-1" : $"{year}-2";
            return semester;
        }

        private static RankingEntryViewModel MapRankingEntry(StudentRegistrationRanking ranking)
        {
            var student = ranking.Student;
            return new RankingEntryViewModel
            {
                Id = ranking.Id,
                StudentRegistrationId = ranking.StudentRegistrationId,
                Semester = ranking.Semester,
                SubjectValue = ranking.SubjectValue,
                RankPosition = ranking.RankPosition,
                Classification = ranking.Classification,
                Status = ranking.Status,
                TotalScore = ranking.TotalScore,
                PerformanceCoefficient = ranking.PerformanceCoefficient,
                ConfirmBy = ranking.ConfirmBy,
                RegistrationDate = ranking.RegistrationDate,
                Student = student == null ? null : new RankingStudentViewModel
                {
                    Id = student.Id,
                    Name = student.Name,
                    RA = student.RA,
                    CPF = student.CPF,
                    RG = student.RG,
                    Cellphone = student.Cellphone,
                    Email = student.User?.Email,
                    CurrentCourse = student.CurrentCourse == null ? null : new RankingCourseViewModel
                    {
                        Description = student.CurrentCourse.Description,
                        Mode = student.CurrentCourse.Mode,
                        Period = student.CurrentCourse.Period,
                        Campus = student.CurrentCourse.Campus
                    }
                }
            };
        }

        private static async Task ApplyDeadlineAsync(PrincipalDataContext context, string semester)
        {
            var now = DateTime.UtcNow;
            var expired = await context.StudentRegistrationRankings
                .Where(r => r.Semester == semester
                    && r.Classification == ClassificationClassificado
                    && r.Status == StatusPendente
                    && r.ConfirmBy != null
                    && r.ConfirmBy < now)
                .ToListAsync();

            if (expired.Count == 0)
                return;

            foreach (var ranking in expired)
            {
                ranking.Status = StatusExpirado;
                ranking.StatusUpdatedAt = now;
                ranking.UpdatedAt = now;
            }

            await context.SaveChangesAsync();
        }

        private static async Task PromoteNextAsync(
            PrincipalDataContext context,
            string semester,
            short subjectValue,
            DateTime? confirmBy)
        {
            if (confirmBy.HasValue && confirmBy.Value < DateTime.UtcNow)
                return;

            var next = await context.StudentRegistrationRankings
                .Where(r => r.Semester == semester
                    && r.SubjectValue == subjectValue
                    && r.Classification == ClassificationEspera
                    && (r.Status == StatusEspera || r.Status == StatusPendente))
                .OrderBy(r => r.RankPosition)
                .FirstOrDefaultAsync();

            if (next == null)
                return;

            next.Classification = ClassificationClassificado;
            next.Status = StatusPendente;
            next.StatusUpdatedAt = DateTime.UtcNow;
            next.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();
        }
    }
}
