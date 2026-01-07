using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.DataContext;
using PlataformaGestaoIA.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlataformaGestaoIA.Controllers.Functions
{
    public class StudentRegistrationSkillControllerFunction
    {
        public static async Task Update(IList<StudentRegistrationSkill> oldStudentRegistrationSkill,
            IList<StudentRegistrationSkill> newStudentRegistrationSkill, PrincipalDataContext context)
        {
            var existingSkills = oldStudentRegistrationSkill;
            var updatedSkills = newStudentRegistrationSkill;

            // Coletar habilidades a serem adicionadas
            var skillsToAdd = new List<StudentRegistrationSkill>();
            foreach (var updatedSkill in updatedSkills)
            {
                if (!existingSkills.Any(es => es.Skill.Tag == GeneralFunction.GenerateTag(updatedSkill.Skill.Name)))
                {
                    await AddStudentSkill(updatedSkill, context);

                    if (updatedSkill.Level > 0)
                      skillsToAdd.Add(updatedSkill);
                }
            }

            // Coletar habilidades a serem removidas
            var skillsToRemove = new List<StudentRegistrationSkill>();
            foreach (var existingSkill in existingSkills)
            {
                if (!updatedSkills.Any(us => GeneralFunction.GenerateTag(us.Skill.Name) == existingSkill.Skill.Tag))
                {
                   if (existingSkill.Level == 0)
                      skillsToRemove.Add(existingSkill);
                }
            }

            // Atualizar coleção original
            foreach (var skillToAdd in skillsToAdd)
            {
                oldStudentRegistrationSkill.Add(skillToAdd);
            }

            foreach (var skillToRemove in skillsToRemove)
            {
                oldStudentRegistrationSkill.Remove(skillToRemove);
            }
        }

        private static async Task AddStudentSkill(StudentRegistrationSkill studentRegistrationSkill, PrincipalDataContext context)
        {
            var tag = GeneralFunction.GenerateTag(studentRegistrationSkill.Skill.Name);
            var existingSkill = await context.Skills.AsTracking().FirstOrDefaultAsync(x => x.Tag == tag);

            if (existingSkill != null)
            {
               studentRegistrationSkill.Skill = existingSkill;
            }
            else
            {
               studentRegistrationSkill.Skill.Tag = GeneralFunction.GenerateTag(studentRegistrationSkill.Skill.Name);
            }
        }

        public static async Task GetExistingSkills(IList<StudentRegistrationSkill> studentRegistrationSkill, PrincipalDataContext context)
        {
            foreach (var existingSkill in studentRegistrationSkill)
            {
                await AddStudentSkill(existingSkill, context);
            }
        }

        public static List<TempStudentRegistrationSkill> DeserializeStudentSkills(string studentSkillsJson)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Ignora maiúsculas/minúsculas nas propriedades
                };

                var tempSkills = JsonSerializer.Deserialize<List<TempStudentRegistrationSkill>>(studentSkillsJson, options);

                if (tempSkills == null)
                {
                    throw new Exception("Falha ao desserializar studentSkills: resultado é nulo.");
                }

                return tempSkills;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao desserializar studentSkills: {ex.Message}");
            }
        }
    }
}