using EmployeeRecordSystem.Client.Helpers;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using EmployeeRecordSystem.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Client.HttpServices
{
    public class EmployeeHttpService
    {
        private readonly HttpClient _httpClient;
        private const string _basePath = "api/employees";

        public EmployeeHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EmployeeDeteilsDto> GetDetailsAsync(Guid employeeId)
        {
            var response = await _httpClient.GetFromJsonAsync<EmployeeDeteilsDto>($"{_basePath}/{employeeId}");
            return response;
        }

        public async Task<List<EmployeeInGroupDto>> GetAllAsync(EmployeeQuery query = null)
        {
            string path = _basePath;

            if (query is not null)
                path = HttpHelper.AddQuery(path, query);

            var response = await _httpClient.GetFromJsonAsync<List<EmployeeInGroupDto>>(path);
            return response;
        }

        public async Task<List<BalanceLogDto>> GetBalanceLogAsync(Guid employeeId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<BalanceLogDto>>($"{_basePath}/{employeeId}/balance-log");
            return response;
        }

        public async Task EditAsync(Guid employeeId, EditEmployeeRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_basePath}/{employeeId}", request);
        }

        public async Task ChangeHourlyPayAsync(Guid employeeId, ChangeEmployeeHourlyPayRequest request)
        {
            var content = HttpHelper.ToHttpContent(request);
            var response = await _httpClient.PatchAsync($"{_basePath}/{employeeId}/hourly-pay", content);
        }

        public async Task ChangeWorkTimeAsync(ChangeEmployeesWorkTimeRequest request)
        {
            var content = HttpHelper.ToHttpContent(request);
            var response = await _httpClient.PatchAsync($"{_basePath}/work-time", content);
        }

        public async Task ConvertWorkTimeToBalanceAsync(ConvertTimeRequest request = null)
        {
            var content = HttpHelper.ToHttpContent(request);
            var response = await _httpClient.PatchAsync($"{_basePath}/work-time/convert", content);
        }
    }
}
