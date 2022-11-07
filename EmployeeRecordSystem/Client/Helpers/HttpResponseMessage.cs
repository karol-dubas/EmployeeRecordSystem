using System.Net.Http.Json;
using EmployeeRecordSystem.Shared.Responses;

namespace EmployeeRecordSystem.Client.Helpers;

public class HttpResponse : HttpResponseMessage
{
	public HttpResponse(HttpResponseMessage message) : base(message.StatusCode)
	{
		if (!IsSuccessStatusCode)
			Errors = message.Content.ReadFromJsonAsync<List<ErrorModel>>().Result;
	}

	public List<ErrorModel> Errors { get; }
}

public class HttpResponse<TContent> : HttpResponse
{
	public HttpResponse(HttpResponseMessage message) : base(message)
	{
		if (IsSuccessStatusCode)
			DeserializedContent = message.Content.ReadFromJsonAsync<TContent>().Result;
	}

	public TContent DeserializedContent { get; }
}
