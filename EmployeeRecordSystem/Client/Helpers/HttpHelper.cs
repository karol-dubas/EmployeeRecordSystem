using EmployeeRecordSystem.Shared.Requests;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Client.Helpers
{
    public static class HttpHelper
    {
        public static string AddQuery<TQuery>(string basePath, TQuery query)
        {
            var querParams = new Dictionary<string, string>();

            foreach (var prop in query.GetType().GetProperties())
            {
                object value = prop.GetValue(query);

                // Skip if reference type default value
                if (value is null)
                    continue;

                // Skip if value type default value
                object propDefaultValue = GetValueTypeDefaultValue(value);
                if (value.Equals(propDefaultValue))
                    continue;

                querParams.Add(prop.Name, value.ToString());
            }

            return QueryHelpers.AddQueryString(basePath, querParams);
        }

        public static StringContent ToHttpContent<TRequest>(TRequest request)
        {
            var serializedRequest = JsonConvert.SerializeObject(request);
            return new StringContent(serializedRequest, Encoding.UTF8, "application/json-patch+json");
        }

        private static object GetValueTypeDefaultValue(object value)
        {
            Type type = value.GetType();

            if (type == typeof(string))
                return null;
                
            return Activator.CreateInstance(type);
        }
    }
}
