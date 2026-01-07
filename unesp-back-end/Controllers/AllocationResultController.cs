using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.DataContext;
using PlataformaGestaoIA.ViewModel;
using Microsoft.AspNetCore.Authorization;
using PlataformaGestaoIA.Models;
using PlataformaGestaoIA.Controllers.Functions;

namespace PlataformaGestaoIA.Controllers
{
    /* [ApiController]
     [Authorize]
     public class AllocationResultController : ControllerBase
     {
         private readonly PrincipalDataContext _context;

         public AllocationResultController(PrincipalDataContext context)
         {
             _context = context;
         }

         // Create a new AllocationResult
         [HttpGet("api/v1/GetAllocationResult/{semester}")]
         [ProducesResponseType(typeof(ResultViewModel<AllocationResult>), 200)]
         [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
         [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
         public async Task<IActionResult> GetAllocationResultSemester([FromRoute] string semester)
         {
             {
                 var studentsSkills = await StudentControllerFunction.GetStudentsSkillsBySemesterAsync(semester, _context);
                 var projectsSkills = await ProjectControllerFunction.GetProjectsSkillsAsync(_context);

                 var studentsList = await StudentControllerFunction.GetIdsAsync(_context);
                 var projectsList = await ProjectControllerFunction.GetIdsAsync(_context);

                 int maxStudentsPerProject = 3;

                 AllocationOptimizer2 optimizer = new AllocationOptimizer2(studentsSkills, projectsSkills, projectsList, studentsList, maxStudentsPerProject);

                 Dictionary<int, List<int>> result = optimizer.OptimizeAllocation();

                 await AllocationResultsFunction.DeleteAllocationResults(_context);
                 await AllocationResultsFunction.SaveAllocationResults(result, semester, _context);

                 /* var AllocationResult = await _context.AllocationResults
                     .Include(ar => ar.Student)
                     .Include(ar => ar.Project)
                     .Where(x => x.Semester == semester)
                     .ToListAsync();

                 var projectsWithStudents = await _context.AllocationResults
                     .Where(ar => ar.Semester == semester)
                     .Include(ar => ar.Project)
                     .Include(ar => ar.Student)
                     .GroupBy(ar => ar.Project)
                     .Select(group => new ProjectWithStudentsViewModel
                     {
                         Project = group.Key,
                         Students = group.Select(ar => ar.Student).ToList()
                     })
                     .ToListAsync();

                 return Ok(new ResultViewModel<List<ProjectWithStudentsViewModel>>(projectsWithStudents));
             }
         }
     }*/
}