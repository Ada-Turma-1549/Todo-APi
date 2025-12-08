using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace PrimeiraApi
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder();
            // 1 - Configurar a Injeção de Dependência: sempre feito no builder.Services
            builder.Services.AddControllers();
            builder.Services.AddDbContext<AppDbContext>();

            var app = builder.Build();
            // 2 - Aplicar/Usar as Dependências: app
            app.MapControllers();

            app.Run();
        }
    }
}
