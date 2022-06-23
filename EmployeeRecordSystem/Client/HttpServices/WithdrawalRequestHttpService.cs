using EmployeeRecordSystem.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using EmployeeRecordSystem.Client.Helpers;
using EmployeeRecordSystem.Shared.Requests;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using EmployeeRecordSystem.Shared.Queries;

namespace EmployeeRecordSystem.Client.HttpServices
{
    public class WithdrawalRequestHttpService
    {
        private readonly HttpClient _httpClient;
        private const string _basePath = "api/withdrawal-requests";

        public WithdrawalRequestHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CreatedWithdrawalRequestDto> CreateAsync(Guid employeeId, CreateWithdrawalRequestRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_basePath}/{employeeId}", request);
            return await response.Content.ReadFromJsonAsync<CreatedWithdrawalRequestDto>();
        }

        public async Task<List<WithdrawalRequestDto>> GetAllAsync(WithdrawalRequestQuery query = null)
        {
            string path = _basePath;

            if (query is not null)
                path = HttpHelper.AddQuery(path, query);

            var response = await _httpClient.GetFromJsonAsync<List<WithdrawalRequestDto>>(path);
            return response;
        }

        public async Task ProcessAsync(Guid withdrawalRequestId, ProcessWithdrawalRequestRequest request)
        {
            var content = HttpHelper.ToHttpContent(request);
            var response = await _httpClient.PatchAsync($"{_basePath}/{withdrawalRequestId}", content);
        }
    }
}
