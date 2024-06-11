﻿using FrequentContentScrappingFunction.Models;
using Microsoft.EntityFrameworkCore;

namespace FrequentContentScrappingFunction;

public sealed class AIChatbotDbContext : DbContext
{
    public DbSet<PageConfiguration> PageConfigurations { get; set; } = null!;

    public AIChatbotDbContext(DbContextOptions<AIChatbotDbContext> options)
        : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PageConfiguration>(entity =>
        {
            entity.HasKey(x => x.Id);
        });
    }
}