using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.DataContext;
using PlataformaGestaoIA.Models;

namespace PlataformaGestaoIA.Controllers.Functions
{
    public class CurrentCourseFunction
    {
        public async static Task GetExisting(CurrentCourse currentCourse, int currentCourseId, PrincipalDataContext context)
        {
            currentCourse = await context.CurrentCourse.FirstOrDefaultAsync(x => x.Id == currentCourseId);
        }

        public static void Update(CurrentCourse oldCurrentCourse, CurrentCourse newCurrentCourse)
        {
            oldCurrentCourse.Mode = newCurrentCourse.Mode;
            oldCurrentCourse.Description = newCurrentCourse.Description;
            oldCurrentCourse.Period = newCurrentCourse.Period;
            oldCurrentCourse.Campus = newCurrentCourse.Campus;
        }
    }
}
