using PlataformaGestaoIA.DataContext;
using PlataformaGestaoIA.Models;
using Microsoft.EntityFrameworkCore;

namespace PlataformaGestaoIA.Controllers.Functions
{
    public class StudentRegistrationControllerFunction
    {
        public static async Task<VerifyStudentRegistrationResult> VerifySemester(StudentRegistration studentRegistration, PrincipalDataContext context)
        {
            var existingStudentRegistration = await context.StudentRegistrations
                    .Include(sr => sr.Student)
                    .ThenInclude(ss => ss.CurrentCourse)
                    .Include(sr => sr.StudentRegistrationScore)
                    .Include(sr => sr.StudentSkills)
                    .ThenInclude(ss => ss.Skill)
                    .AsTracking().FirstOrDefaultAsync(sr => sr.Student != null &&
                                sr.Student.RA.Equals(studentRegistration.Student.RA) &&
                                sr.Semester.Equals(studentRegistration.Semester));

            if (existingStudentRegistration != null)
            {

                await Update(existingStudentRegistration, studentRegistration, context);
                return new VerifyStudentRegistrationResult
                {
                    Exists = true,
                    StudentRegistration = existingStudentRegistration

                };
            }

            foreach (var studentSkill in studentRegistration.StudentSkills)
            {
                studentSkill.Skill.Tag = GeneralFunction.GenerateTag(studentSkill.Skill.Name);
            }

            return new VerifyStudentRegistrationResult
            {
                Exists = false,
                StudentRegistration = studentRegistration
            };
        }

        public static async Task TrackEntities(StudentRegistration studentRegistration, PrincipalDataContext context)
        {
            studentRegistration.Student = await StudentControllerFunction.GetExisting(studentRegistration.Student, context);
            await StudentRegistrationSkillControllerFunction.GetExistingSkills(studentRegistration.StudentSkills, context);
        }

        public static async Task Update(StudentRegistration oldStudentRegistration, StudentRegistration newStudentRegistration, PrincipalDataContext context)
        {
            await StudentControllerFunction.Update(oldStudentRegistration.Student, newStudentRegistration.Student, context);
            StudentRegistrationScoreControllerFunction.Update(oldStudentRegistration.StudentRegistrationScore, newStudentRegistration.StudentRegistrationScore);
            await StudentRegistrationSkillControllerFunction.Update(oldStudentRegistration.StudentSkills, newStudentRegistration.StudentSkills, context);
            oldStudentRegistration.SkillsDescription = newStudentRegistration.SkillsDescription;
            oldStudentRegistration.RegistrationDate = newStudentRegistration.RegistrationDate;
            oldStudentRegistration.Subject = newStudentRegistration.Subject;
            oldStudentRegistration.ChoicePriority = newStudentRegistration.ChoicePriority;
            oldStudentRegistration.EnrolledInIndustry4_0 = newStudentRegistration.EnrolledInIndustry4_0;
            oldStudentRegistration.DoesNotMeetRequirements = newStudentRegistration.DoesNotMeetRequirements;
            oldStudentRegistration.FirstMeetingDate = newStudentRegistration.FirstMeetingDate;
            oldStudentRegistration.Semester = newStudentRegistration.Semester;
        }
    }

    public class VerifyStudentRegistrationResult
    {
        public bool Exists { get; set; }
        public StudentRegistration StudentRegistration { get; set; }
    }
}
