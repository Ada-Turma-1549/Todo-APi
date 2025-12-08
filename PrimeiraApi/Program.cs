using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using System;
using System.IO;
using System.Reflection;

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
            builder.Services.AddSwaggerGen((options) =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Todo Items API",
                    Description = "API para gerenciar uma lista de tarefas",
                    Version = "v1",
                    Contact = new OpenApiContact()
                    {
                        Name = "Paulo Ricardo",
                        Email = "paulo@paulo.eti.br",
                        Url = new Uri("https://paulo.bio")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    },
                    Summary = "Uma API simples para gerenciar tarefas",
                    TermsOfService = new Uri("https://example.com/terms")
                });

                var xmlFile = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                options.IncludeXmlComments(xmlFile);
            });

            //Swagger -> ferramenta pra gerar a documentação da API no formato Open API

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();

            // 2 - Aplicar/Usar as Dependências: app
            app.MapControllers();

            app.Run();
        }
    }
}
