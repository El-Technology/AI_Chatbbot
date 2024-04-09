using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;
using WebScrapperFunction.Models;

namespace WebScrapperFunction.Accessors;

public sealed class AIChatbotDbContext : DbContext
{
    public DbSet<ResourcesModel> ResourcesModels { get; set; } = null!;

    public AIChatbotDbContext(DbContextOptions<AIChatbotDbContext> options)
        : base(options)
    {
       
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(o => o.UseVector());
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