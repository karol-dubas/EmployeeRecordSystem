using System.Net.Http.Json;
using EmployeeRecordSystem.Client.Helpers;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using EmployeeRecordSystem.Shared.Responses;

namespace EmployeeRecordSystem.Client.HttpServices;

public class EmployeeHttpService
{
	private const string _basePath = "api/employees";
	private readonly HttpClient _httpClient;

	public EmployeeHttpService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<HttpResponse<EmployeeDeteilsDto>> GetDetailsAsync(Guid employeeId)
	{
		return (await _httpClient.GetAsync($"{_basePath}/{employeeId}"))
			.DeserializeContent<EmployeeDeteilsDto>();
	}

	public async Task<HttpResponse<List<EmployeeInGroupDto>>> GetAllAsync(EmployeeQuery query = null)
	{
		string path = _basePath;

		if (query is not null)
			path = path.AddHttpQuery(query);

		return (await _httpClient.GetAsync(path))
			.DeserializeContent<List<EmployeeInGroupDto>>();
	}

	public async Task<HttpResponse<PagedContent<BalanceLogDto>>> GetBalanceLogsAsync(Guid employeeId, BalanceLogQuery query = null)
	{
		string path = $"{_basePath}/{employeeId}/balance-log";

		if (query is not null)
			path = path.AddHttpQuery(query);
		
		return (await _httpClient.GetAsync(path))
			.DeserializeContent<PagedContent<BalanceLogDto>>();
	}

	public async Task<HttpResponse> EditAsync(Guid employeeId, EditEmployeeRequest request)
	{
		return (await _httpClient.PutAsJsonAsync($"{_basePath}/{employeeId}", request))
			.DeserializeContent();
	}

	public async Task<HttpResponse> ChangeHourlyPayAsync(Guid employeeId, ChangeEmployeeHourlyPayRequest request)
	{
		return (await _httpClient.PatchAsync($"{_basePath}/{employeeId}/hourly-pay", request.ToHttpContent()))
			.DeserializeContent();
	}

	public async Task<HttpResponse> ChangeWorkTimeAsync(ChangeEmployeesWorkTimeRequest request)
	{
		return (await _httpClient.PatchAsync($"{_basePath}/work-time", request.ToHttpContent()))
			.DeserializeContent();
	}

	public async Task<HttpResponse> ConvertWorkTimeToBalanceAsync(ConvertTimeRequest request)
	{
		return (await _httpClient.PatchAsync($"{_basePath}/work-time/convert", request.ToHttpContent()))
			.DeserializeContent();
	}
}
