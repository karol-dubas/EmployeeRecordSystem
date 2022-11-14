using System.Text;
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

			if (IsRefTypeDefaultValue(value) || IsValueTypeDefaultValue(value))
				continue;

			queryParams.Add(prop.Name, value!.ToString());
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

	private static bool IsValueTypeDefaultValue(object value)
	{
		var type = value.GetType();
		object valObj = type == typeof(string) ? null : Activator.CreateInstance(type);

		return value.Equals(valObj);
	}

	private static bool IsRefTypeDefaultValue(object value)
	{
		return value is null;
	}
}
