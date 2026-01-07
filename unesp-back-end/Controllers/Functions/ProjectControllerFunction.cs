using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.DataContext;

namespace PlataformaGestaoIA.Controllers.Functions
{
    public static class ProjectControllerFunction
    {
        public static async Task<Dictionary<int, double[]>> GetProjectsSkillsAsync(PrincipalDataContext context)
        {
            // Obter todas as skills disponíveis
            var allSkills = await context.Skills
                                         .OrderBy(s => s.Id)
                                         .ToListAsync();

            var projects = await context.Projects
                                         .Include(p => p.ProjectSkill)
                                         .ThenInclude(ps => ps.Skill)
                                         .ToListAsync();

            Dictionary<int, double[]> projectsSkills = new Dictionary<int, double[]>();

            foreach (var project in projects)
            {
                int projectId = project.Id;
                double[] skillLevels = new double[allSkills.Count];

                // Inicializar todas as skills com nível 0
                for (int i = 0; i < allSkills.Count; i++)
                {
                    skillLevels[i] = 0;
                }

                // Atualizar os níveis de skills que o projeto realmente possui
                foreach (var projectSkill in project.ProjectSkill)
                {
                    int skillIndex = allSkills.FindIndex(s => s.Id == projectSkill.Skill.Id);
                    if (skillIndex >= 0)
                    {
                        skillLevels[skillIndex] = (double)projectSkill.Level;
                    }
                }

                projectsSkills.Add(projectId, skillLevels);
            }

            return projectsSkills;
        }

        public static async Task<List<int>> GetIdsAsync(PrincipalDataContext context)
        {
            var projectIds = await context.Projects
                .Select(p => p.Id)
                .ToListAsync();

            return projectIds;
        }
    }
}
