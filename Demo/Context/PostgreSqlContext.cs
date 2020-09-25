using System.IO;
using Demo.Models.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Demo.Context
{
    public class PostgreSqlContext : DbContext
    {
        public DbSet<TipoPedido> TipoPedidos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseNpgsql(config.GetConnectionString("PostgreSql"));
            base.OnConfiguring(optionsBuilder);
        }
    }
}
