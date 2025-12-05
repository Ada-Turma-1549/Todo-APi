using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace PrimeiraApi
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var todos = new TodoItem[]
            {
                new TodoItem { Id = 1, Title = "Primeira Tarefa", Description = "Descrição da Tarefa 1", IsFinished = false },
                new TodoItem { Id = 2, Title = "Segunda Tarefa", Description = "Descrição da Tarefa 2", IsFinished = false },
                new TodoItem { Id = 3, Title = "Terceira Tarefa", Description = "Descrição da Tarefa 3", IsFinished = false },
            };
            SimulatedDatabase.Todos.AddRange(todos);

            var builder = WebApplication.CreateBuilder();
            // 1 - Configurar a Injeção de Dependência: sempre feito no builder.Services
            builder.Services.AddControllers();

            var app = builder.Build();
            // 2 - Aplicar/Usar as Dependências: app
            app.MapControllers();

            app.Run();
        }
    }
}
