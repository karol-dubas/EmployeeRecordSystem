using EmployeeRecordSystem.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using EmployeeRecordSystem.Client.Helpers;

namespace EmployeeRecordSystem.Client.HttpServices
{
    public class RoleHttpService
    {
        private readonly HttpClient _httpClient;
        private const string _basePath = "api/roles";

        public RoleHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponse<List<RoleDto>>> GetAllAsync()
        {
            return (await _httpClient.GetAsync(_basePath))
                .DeserializeContent<List<RoleDto>>();
        }

        public async Task<HttpResponse> ChangeEmployeeRoleAsync(Guid employeeId, Guid newRoleId)
        {
            return (await _httpClient.PatchAsync($"{_basePath}/{newRoleId}/employee/{employeeId}", null))
                .DeserializeContent();
        }
    }
}
