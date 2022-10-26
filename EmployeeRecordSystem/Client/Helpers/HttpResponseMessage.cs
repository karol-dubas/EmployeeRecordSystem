using System.Net.Http.Json;

namespace EmployeeRecordSystem.Client.Helpers;

public class HttpResponse : HttpResponseMessage
{
	public HttpResponse(HttpResponseMessage message) : base(message.StatusCode)
	{
		if (!IsSuccessStatusCode)
			ErrorMessage = message.Content.ReadAsStringAsync().Result;
	}

	public string ErrorMessage { get; }
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
