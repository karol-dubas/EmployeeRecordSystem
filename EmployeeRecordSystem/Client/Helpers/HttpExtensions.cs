using System.Text;
using EmployeeRecordSystem.Shared.Responses;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace EmployeeRecordSystem.Client.Helpers;

public static class HttpExtensions
{
	public static string AddHttpQuery<TQuery>(this string basePath, TQuery query)
	{
		var queryParams = new Dictionary<string, string>();

		foreach (var prop in query.GetType().GetProperties())
		{
			object value = prop.GetValue(query);

			// Skip if reference type default value
			if (value is null)
				continue;

			// Skip if value type default value
			bool isRefTypeDefaultValue = value.Equals(GetValueTypeDefaultValue(value));
			if (isRefTypeDefaultValue)
				continue;

			queryParams.Add(prop.Name, value.ToString());
		}

		return QueryHelpers.AddQueryString(basePath, queryParams);
	}

	public static StringContent ToHttpContent<TRequest>(this TRequest request)
	{
		string serializedRequest = JsonConvert.SerializeObject(request);
		return new StringContent(serializedRequest, Encoding.UTF8, "application/json-patch+json");
	}

	public static HttpResponse<TContent> DeserializeContent<TContent>(this HttpResponseMessage message)
	{
		return new HttpResponse<TContent>(message);
	}
	
	public static HttpResponse DeserializeContent(this HttpResponseMessage message)
	{
		return new HttpResponse(message);
	}

	private static object GetValueTypeDefaultValue(object value)
	{
		var type = value.GetType();
		return type == typeof(string) ? null : Activator.CreateInstance(type);
	}
}
