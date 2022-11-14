using EmployeeRecordSystem.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeRecordSystem.Server.ErrorHandlers;

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
		
		var errorResponse = ParseErrors(errorsInModelState);
		context.Result = new BadRequestObjectResult(errorResponse);
	}

	private static List<ErrorModel> ParseErrors(IEnumerable<KeyValuePair<string, IEnumerable<string>>> errorsInModelState)
	{
		var errorResponse = new List<ErrorModel>();

		foreach (var error in errorsInModelState)
		{
			var subErrors = error.Value.Select(subError => new ErrorModel() 
			{
				FieldName = error.Key,
				Message = subError
			});
			
			errorResponse.AddRange(subErrors);
		}
		
		return errorResponse;
	}
}