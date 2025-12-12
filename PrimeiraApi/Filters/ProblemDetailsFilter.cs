using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace PrimeiraApi.Filters
{
    public class ProblemDetailsFilter : IAlwaysRunResultFilter
    {
        private readonly ProblemDetailsFactory problemDetailsFactory;

        public ProblemDetailsFilter(ProblemDetailsFactory problemDetailsFactory)
        {
            this.problemDetailsFactory = problemDetailsFactory;
        }

        public void OnResultExecuted(ResultExecutedContext context) { }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            int statusCode = 0;
            string message = null;

            if (context.Result is StatusCodeResult statusCodeResult)
            {
                statusCode = statusCodeResult.StatusCode;
            }
            else if (context.Result is ObjectResult objectResult) // return BadRequest("message")
            {
                if (objectResult.Value is ProblemDetails)
                    return;

                if (objectResult.Value is string resultMessage)
                    message = resultMessage;

                if (objectResult.StatusCode.HasValue)
                    statusCode = objectResult.StatusCode.Value;
            }

            if (statusCode >= 400)
            {
                var problemdetails = problemDetailsFactory.CreateProblemDetails(
                    context.HttpContext,
                    statusCode: statusCode,
                    detail: message,
                    instance: context.HttpContext.Request.Path
                );

                context.Result = new ObjectResult(problemdetails)
                {
                    StatusCode = statusCode
                };
            }
        }
    }
}
