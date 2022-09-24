using EmployeeRecordSystem.Server.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(e.Message);
			}
            catch (InvalidOperationException e)
            {
                context.Response.StatusCode = 409;
                await context.Response.WriteAsync(e.Message);
            }
            catch (ArgumentOutOfRangeException e)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(e.Message);
            }
        }
    }
}
