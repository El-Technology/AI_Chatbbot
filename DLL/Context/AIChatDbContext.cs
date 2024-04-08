using Common;
using DLL.Models;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;

namespace DLL.Context;

public sealed class AIChatbotDbContext : DbContext
{
    public DbSet<ResourcesModel> ResourcesModels { get; set; } = null!;

    public AIChatbotDbContext() { }
    public AIChatbotDbContext(DbContextOptions<AIChatbotDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(EnvironmentVariables.ConnectionString, o => o.UseVector());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");

        modelBuilder.Entity<ResourcesModel>(entity =>
        {
            entity.ToTable("ResourcesModel");
            entity.HasKey(x => x.Id);
        });
    }
}