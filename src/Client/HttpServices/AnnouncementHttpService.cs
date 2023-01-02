using System.Net.Http.Json;
using EmployeeRecordSystem.Client.Helpers;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using EmployeeRecordSystem.Shared.Responses;

namespace EmployeeRecordSystem.Client.HttpServices;

public class AnnouncementHttpService
{
	private const string _basePath = "api/announcements";
	private readonly HttpClient _httpClient;

	public AnnouncementHttpService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<HttpResponse<AnnouncementDto>> CreateAsync(CreateAnnouncementRequest request)
	{
		return (await _httpClient.PostAsJsonAsync(_basePath, request))
			.ToHttpResponseWithContent<AnnouncementDto>();
	}

	public async Task<HttpResponse<List<AnnouncementDto>>> GetAllAsync(AnnouncementQuery query = null)
	{
		string path = _basePath;

		if (query is not null)
			path = path.AddHttpQuery(query);

		return (await _httpClient.GetAsync(path))
			.ToHttpResponseWithContent<List<AnnouncementDto>>();
	}

	public async Task<HttpResponse> UpdateAsync(Guid announcementId, CreateAnnouncementRequest request)
	{
		return (await _httpClient.PutAsync($"{_basePath}/{announcementId}", request.ToHttpContent()))
			.ToHttpResponse();
	}

	public async Task<HttpResponse> DeleteAsync(Guid announcementId)
	{
		return (await _httpClient.DeleteAsync($"{_basePath}/{announcementId}"))
			.ToHttpResponse();
	}
}
