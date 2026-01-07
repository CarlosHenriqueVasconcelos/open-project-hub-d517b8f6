using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.DataContext;
using PlataformaGestaoIA.Models;

namespace PlataformaGestaoIA.Controllers.Functions
{
    public class CompanyRepresentativeControllerFunction
    {
        public static async Task TrackEntities(CompanyRepresentative companyRepresentative, PrincipalDataContext context)
        {
            companyRepresentative.Company = await CompanyControllerFunction.GetExisting(companyRepresentative.Company, context);
            companyRepresentative.User = await UserControllerFunction.GetExisting(companyRepresentative.User, context);
        }
    }
}
