using System.Net.Http.Json;
using EmployeeRecordSystem.Client.Helpers;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using EmployeeRecordSystem.Shared.Responses;

namespace EmployeeRecordSystem.Client.HttpServices;

public class WithdrawalRequestHttpService
{
	private const string _basePath = "api/withdrawal-requests";
	private readonly HttpClient _httpClient;

	public WithdrawalRequestHttpService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<HttpResponse<CreatedWithdrawalRequestDto>> CreateAsync(
		Guid employeeId,
		CreateWithdrawalRequestRequest request)
	{
		return (await _httpClient.PostAsJsonAsync($"{_basePath}/{employeeId}", request))
			.DeserializeContent<CreatedWithdrawalRequestDto>();
	}

	public async Task<HttpResponse<List<WithdrawalRequestDto>>> GetAllAsync(WithdrawalRequestQuery query = null)
	{
		string path = _basePath;

		if (query is not null)
			path = path.AddHttpQuery(query);

		return (await _httpClient.GetAsync(path))
			.DeserializeContent<List<WithdrawalRequestDto>>();
	}

	public async Task<HttpResponse> ProcessAsync(Guid withdrawalRequestId, ProcessWithdrawalRequestRequest request)
	{
		return (await _httpClient.PatchAsync($"{_basePath}/{withdrawalRequestId}", request.ToHttpContent()))
			.DeserializeContent();
	}
}
