using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace PrimeiraApi.Middlewares
{
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IConfiguration configuration;

        public MaintenanceMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            this.next = next;
            this.configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var maintenanceEnabled = configuration.GetValue<bool>("Maintenance:Enabled");
            var maintenanceMessage = configuration.GetValue<string>("Maintenance:Message");
            var retryAt = configuration.GetValue<int>("Maintenance:RetryAfter");


            if (maintenanceEnabled)
            {
                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status503ServiceUnavailable,
                    Title = "Serviço Indisponível - Manutenção Programada",
                    Detail = maintenanceMessage,
                    Type = "https://httpstatuses.com/503",
                    Instance = context.Request.Path
                };

                problem.Extensions["retry-after"] = retryAt;
                problem.Extensions["trace-id"] = context.TraceIdentifier;

                await context.Response.WriteAsJsonAsync(problem);
                return;
            }

            await next(context);
        }
    }
}
