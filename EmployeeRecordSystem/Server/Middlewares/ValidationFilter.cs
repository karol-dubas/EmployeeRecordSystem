using EmployeeRecordSystem.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeRecordSystem.Server.Middlewares;

public class ValidationFilter : IAsyncActionFilter
{
	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		if (context.ModelState.IsValid)
			await next();
			
		var errorsInModelState = context.ModelState
			.Where(kvp => kvp.Value.Errors.Count > 0)
			.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage))
			.ToArray();
		
		var errorResponse = new List<ErrorModel>();

		foreach (var error in errorsInModelState)
		{
			foreach (string subError in error.Value)
			{
				var errorModel = new ErrorModel()
				{
					FieldName = error.Key,
					Message = subError
				};
				
				errorResponse.Add(errorModel);
			}
		}

		context.Result = new BadRequestObjectResult(errorResponse);
	}
}