using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace PlataformaGestaoIA.Attributes
{
	[AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
	public class ApiKeyAttribute : Attribute, IAsyncActionFilter
	{
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if (!context.HttpContext.Request.Query.TryGetValue(Configuration.ApiKeyName, out var _apiKeyName))
			{
				context.Result = new ContentResult()
				{
					StatusCode = 401,
					Content = "Não foi passado o parâmetro"
				};

				return;
			}

			if (!Configuration.ApiKeyValue.Equals(_apiKeyName))
			{
				context.Result = new ContentResult()
				{
					StatusCode = 403,
					Content = "Parâmetro errado"
				};

				return;
			}


			await next();
		}
	}
}
