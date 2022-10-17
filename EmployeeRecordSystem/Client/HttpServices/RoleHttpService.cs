using EmployeeRecordSystem.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<RoleDto>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<RoleDto>>(_basePath);
        }

        public async Task ChangeEmployeeRoleAsync(Guid employeeId, Guid roleId)
        {
            var response = await _httpClient.PatchAsync($"{_basePath}/{roleId}/employee/{employeeId}", null);
        }
    }
}
