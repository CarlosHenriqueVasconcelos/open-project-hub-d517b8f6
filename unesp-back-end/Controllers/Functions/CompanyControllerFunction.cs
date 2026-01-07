using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.DataContext;
using PlataformaGestaoIA.Models;

namespace PlataformaGestaoIA.Controllers.Functions
{
    public class CompanyControllerFunction
    {
        public async static Task<Company> GetExisting(Company company, PrincipalDataContext context)
        {
            var existingCompany = await context.Companies.FirstOrDefaultAsync(x => x.CNPJ == company.CNPJ);

            if (existingCompany != null)
            {
                return existingCompany;
            }

            return company;
        }
    }
}
