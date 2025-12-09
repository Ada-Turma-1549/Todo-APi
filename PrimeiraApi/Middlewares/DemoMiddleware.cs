using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Threading.Tasks;

namespace PrimeiraApi.Middlewares
{
    // O que faz uma classe ser um middleware?
    // 1 - ter um construtor que requece um RequestDelegate
    // 2 - ter um método chamado InvokeAsync que recebe um HttpContext como parâmetro
    public class DemoMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<DemoMiddleware> logger;

        public DemoMiddleware(RequestDelegate next, ILogger<DemoMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Adiciona a lógica do Middleware 
            // - Verificar se o usuário tem a api contratada no plano dele
            // - verificar se o dado já está no cache
            // - verificar o rate limiting
            
            logger.LogError("Lógica que é executada ANTES de a request seguir para o próximo");
            await next(context);
            logger.LogError("Lógica que é executada DEPOIS que request voltou do próximo");
        }
    }
}
