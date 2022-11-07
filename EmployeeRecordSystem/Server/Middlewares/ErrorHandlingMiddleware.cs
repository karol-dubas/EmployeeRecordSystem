using EmployeeRecordSystem.Server.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using EmployeeRecordSystem.Shared.Responses;

namespace EmployeeRecordSystem.Server.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await context.Response.WriteAsJsonAsync(CreateErrorMessage(e));
            }
            catch (BadRequestException e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(CreateErrorMessage(e));
            }
            catch (ForbidException e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsJsonAsync(CreateErrorMessage(e));
            }
            catch (Exception)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }

        private static List<ErrorModel> CreateErrorMessage(IHttpException e)
        {
            var errors = new List<ErrorModel>
            {
                new()
                {
                    FieldName = e.FieldName,
                    Message = e.Message
                }
            };
            
            return errors;
        }
    }
}
