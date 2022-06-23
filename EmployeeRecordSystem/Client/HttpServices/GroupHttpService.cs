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
    public class GroupHttpService
    {
        private readonly HttpClient _httpClient;
        private const string _basePath = "api/groups";

        public GroupHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GroupDto> CreateAsync(CreateGroupRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(_basePath, request);
            return await response.Content.ReadFromJsonAsync<GroupDto>();
        }

        public async Task<List<GroupDto>> GetAllAsync(GroupQuery query = null)
        {
            string path = _basePath;

            if (query is not null)
                path = HttpHelper.AddQuery(path, query);

            var response = await _httpClient.GetFromJsonAsync<List<GroupDto>>(path);
            return response;
        }

        public async Task<GroupDto> RenameAsync(Guid groupId, RenameGroupRequest request)
        {
            var content = HttpHelper.ToHttpContent(request);
            var response = await _httpClient.PatchAsync($"{_basePath}/{groupId}", content);
            return await response.Content.ReadFromJsonAsync<GroupDto>(); ;
        }
        
        public async Task AssignEmployeeToGroupAsync(Guid groupId, Guid employeeId)
        {
            var response = await _httpClient.PatchAsync($"{_basePath}/{groupId}/employee/{employeeId}", null);
        }
        
        public async Task RemoveEmployeeFromGroupAsync(Guid employeeId)
        {
            var response = await _httpClient.DeleteAsync($"{_basePath}/employee/{employeeId}");
        }
        
        public async Task DeleteAsync(Guid groupId)
        {
            var response = await _httpClient.DeleteAsync($"{_basePath}/{groupId}");
        }
    }
}
