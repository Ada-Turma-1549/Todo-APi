using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using System.Linq;

namespace PrimeiraApi.Filters
{
    public class TenantFilter : IResourceFilter
    {
        private readonly TenantConfiguration tenantCredentials;
        private readonly ProblemDetailsFactory problemFactory;

        public TenantFilter(IOptions<TenantConfiguration> options, ProblemDetailsFactory problemFactory)
        {
            this.tenantCredentials = options.Value ?? new TenantConfiguration();
            this.problemFactory = problemFactory;
        }

        public void OnResourceExecuted(ResourceExecutedContext context) { }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var headers = context.HttpContext.Request.Headers;

            // O usuário não passa o Tenant ID
            // O usuário não passa o API KEY
            // O usuário passa o API KEY E o Tenant ID, mas estão incorretos

            var hasTenant = headers.TryGetValue("X-Tenant-Id", out var tenantId);
            var hasApiKey = headers.TryGetValue("X-API-KEY", out var apiKey);

            if (!hasTenant || !hasApiKey)
            {
                var problemResult = problemFactory.CreateProblemDetails(
                    context.HttpContext,
                    statusCode: StatusCodes.Status401Unauthorized,
                    detail: "API Key ou Tenant ID não informados",
                    instance: context.HttpContext.Request.Path
                );

                context.Result = new ObjectResult(problemResult)
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }

            var isValid = tenantCredentials.Tenants.Any(x => x.Key == tenantId && x.Value == apiKey);
            if (!isValid)
            {
                var problemResult = problemFactory.CreateProblemDetails(
                  context.HttpContext,
                  statusCode: StatusCodes.Status403Forbidden,
                  detail: "API Key e Tenant ID inválidos",
                  instance: context.HttpContext.Request.Path
                );

                context.Result = new ObjectResult(problemResult)
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
        }
    }
}
