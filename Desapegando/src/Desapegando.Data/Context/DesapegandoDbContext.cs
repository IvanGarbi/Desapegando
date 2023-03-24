using Microsoft.EntityFrameworkCore;
using Desapegando.Business.Models;

namespace Desapegando.Data.Context;

public class DesapegandoDbContext : DbContext
{
    public DesapegandoDbContext(DbContextOptions<DesapegandoDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Condomino> Condominos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetProperties()
                         .Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("VARCHAR(50)");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DesapegandoDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}