using System.Net.Http.Json;
using EmployeeRecordSystem.Client.Helpers;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using EmployeeRecordSystem.Shared.Responses;

namespace EmployeeRecordSystem.Client.HttpServices;

public class GroupHttpService
{
	private const string _basePath = "api/groups";
	private readonly HttpClient _httpClient;

	public GroupHttpService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<HttpResponse<GroupDto>> CreateAsync(CreateGroupRequest request)
	{
		return (await _httpClient.PostAsJsonAsync(_basePath, request))
			.ToHttpResponseWithContent<GroupDto>();
	}

	public async Task<HttpResponse<List<GroupDto>>> GetAllAsync(GroupQuery query = null)
	{
		string path = _basePath;

		if (query is not null)
			path = path.AddHttpQuery(query);

		return (await _httpClient.GetAsync(path))
			.ToHttpResponseWithContent<List<GroupDto>>();
	}

	public async Task<HttpResponse<GroupDto>> RenameAsync(Guid groupId, RenameGroupRequest request)
	{
		return (await _httpClient.PatchAsync($"{_basePath}/{groupId}", request.ToHttpContent()))
			.ToHttpResponseWithContent<GroupDto>();
	}

	public async Task<HttpResponse> AssignEmployeeToGroupAsync(Guid groupId, Guid employeeId)
	{
		return (await _httpClient.PatchAsync($"{_basePath}/{groupId}/employee/{employeeId}", null))
			.ToHttpResponse();
	}

	public async Task<HttpResponse> RemoveEmployeeFromGroupAsync(Guid employeeId)
	{
		return (await _httpClient.DeleteAsync($"{_basePath}/employee/{employeeId}"))
			.ToHttpResponse();
	}

	public async Task<HttpResponse> DeleteAsync(Guid groupId)
	{
		return (await _httpClient.DeleteAsync($"{_basePath}/{groupId}"))
			.ToHttpResponse();
	}
}
