using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.DataContext;
using PlataformaGestaoIA.Models;

namespace PlataformaGestaoIA.Controllers.Functions
{
    public static class AllocationResultsFunction
    {
        public static async Task DeleteAllocationResults(PrincipalDataContext context)
        {
            var entitiesToRemove = context.AllocationResults.ToList();
            context.AllocationResults.RemoveRange(entitiesToRemove);
            context.SaveChanges();
        }

        public static async Task SaveAllocationResults(Dictionary<int, List<int>> allocationResults, string semester, PrincipalDataContext context)
        {
            foreach (var projectAllocation in allocationResults)
            {
                int projectId = projectAllocation.Key;
                List<int> studentIds = projectAllocation.Value;

                foreach (var studentId in studentIds)
                {
                    var allocationResult = new AllocationResult
                    {
                        Student = await context.Students.FindAsync(studentId),
                        Project = await context.Projects.FindAsync(projectId),
                        Semester = semester
                    };

                    context.AllocationResults.Add(allocationResult);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
