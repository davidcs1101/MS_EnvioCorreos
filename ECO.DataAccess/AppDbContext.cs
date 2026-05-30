using Microsoft.EntityFrameworkCore;
using ECO.Dominio.Entidades;

namespace ECO.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder){}

        public DbSet<ECO_ColaSolicitud> ECO_ColaSolicitudes { get; set; }
    }
}
