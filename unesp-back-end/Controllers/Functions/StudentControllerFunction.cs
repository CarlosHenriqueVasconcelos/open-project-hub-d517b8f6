using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.DataContext;
using PlataformaGestaoIA.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PlataformaGestaoIA.Controllers.Functions
{
    public class StudentControllerFunction
    {
        public static async Task Update(Student oldStudent, Student newStudent, PrincipalDataContext context)
        {
            oldStudent.Name = newStudent.Name;
            oldStudent.RA = newStudent.RA;
            oldStudent.CPF = newStudent.CPF;
            oldStudent.RG = newStudent.RG;
            oldStudent.Cellphone = newStudent.Cellphone;
            oldStudent.User = newStudent.User;
            oldStudent.User.Name = newStudent.Name;
            await CurrentCourseFunction.GetExisting(oldStudent.CurrentCourse, oldStudent.CurrentCourse.Id, context);
            CurrentCourseFunction.Update(oldStudent.CurrentCourse, newStudent.CurrentCourse);
        }

        public static async Task<Student> GetExisting(Student student, PrincipalDataContext context)
        {
            var slug = GeneralFunction.GenerateTag(student.User.Email);
            var existingStudent = await context
                .Students
                .Include(x => x.User)
                .AsTracking()
                .FirstOrDefaultAsync(x => x.User.Slug == slug);

            if (existingStudent != null)
                return existingStudent;
            else
            {
                student.User.Name = student.Name;
                student.User.Slug = GeneralFunction.GenerateTag(student.User.Email);
                //await AddStudentRoleAsync(student, context);
                //student.User.Roles.Add(await RoleControllerFunction.ReturnRole("Student", context));
                return student;
            }
        }

        private static async Task AddStudentRoleAsync(Student student, PrincipalDataContext context)
        {
            if (student.User.Roles == null)
            {
                student.User.Roles = new List<Role>();
            }

            var role = await RoleControllerFunction.ReturnRole("Student", context);

            // Adiciona a nova role à lista de roles do usuário
            if (!student.User.Roles.Any(r => r.Slug == role.Slug))
            {
                student.User.Roles.Add(role);

                // Se a role não estava no banco de dados, salve-a
                if (role.Id == 0) // Assumindo que o Id é zero para roles que não estão persistidas no banco de dados
                {
                    context.Role.Add(role);
                }

                await context.SaveChangesAsync();
            }
        }

        public static async Task<Dictionary<int, double[]>> GetStudentsSkillsBySemesterAsync(string semester, PrincipalDataContext context)
        {
            // Obter todas as skills disponíveis
            var allSkills = await context.Skills
                                         .OrderBy(s => s.Id)
                                         .ToListAsync();

            var studentRegistrations = await context.StudentRegistrations
                .Include(sr => sr.Student)
                .Include(sr => sr.StudentSkills)
                    .ThenInclude(ss => ss.Skill)
                .Where(sr => sr.Semester == semester)
                .ToListAsync();

            Dictionary<int, double[]> studentsSkills = new Dictionary<int, double[]>();

            foreach (var registration in studentRegistrations)
            {
                int studentId = registration.Student.Id;
                double[] skillLevels = new double[allSkills.Count];

                // Inicializar todas as skills com nível 0
                for (int i = 0; i < allSkills.Count; i++)
                {
                    skillLevels[i] = 0;
                }

                // Atualizar os níveis de skills que o estudante realmente possui
                foreach (var studentSkill in registration.StudentSkills)
                {
                    int skillIndex = allSkills.FindIndex(s => s.Id == studentSkill.Skill.Id);
                    if (skillIndex >= 0)
                    {
                        skillLevels[skillIndex] = (double)studentSkill.Level;
                    }
                }

                studentsSkills.Add(studentId, skillLevels);
            }

            return studentsSkills;
        }

        public static async Task<List<int>> GetIdsAsync(PrincipalDataContext context)
        {
            var studentIds = await context.Students
                .Select(s => s.Id)
                .ToListAsync();

            return studentIds;
        }
    }
}
