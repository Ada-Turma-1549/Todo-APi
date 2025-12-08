using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace PrimeiraApi
{
    public class AppDbContext : DbContext
    {
        public DbSet<TodoItem> Todos => Set<TodoItem>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "todoapi.db");
            optionsBuilder.UseSqlite($"Data Source={path}");
            // String Conexões: Texto que contém os detalhes para conectar no banco (ip/host, usuário, senha, etc)

            base.OnConfiguring(optionsBuilder);
        }
    }
}
