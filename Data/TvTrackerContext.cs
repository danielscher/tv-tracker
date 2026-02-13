using Microsoft.EntityFrameworkCore;
using TvTracker.Exception;
using TvTracker.Models;

namespace TvTracker.Data;

public class TvTrackerContext(DbContextOptions<TvTrackerContext> options) : DbContext(options)
{
    public DbSet<Profile> Profiles {get; set;}
    public DbSet<UserMedia> UserMedia{get;set;}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureUserMedia(modelBuilder);

        modelBuilder.Entity<Movie>()
        .ToTable("Movies")
        .OwnsOne(m=>m.MetaInfo);            // embed meta info

        modelBuilder.Entity<Series>()
        .ToTable("Series")
        .OwnsOne(s => s.MetaInfo);

        modelBuilder.Entity<Series>()
        .HasMany(s => s.Seasons);

        modelBuilder.Entity<Season>()
        .ToTable("Seasons");
    }

    /// <summary>
    /// Configures the UserMedia Hierarchy relationships
    /// </summary>
    private void ConfigureUserMedia(ModelBuilder modelBuilder)
    {
        // UserMedia - profile relation
        modelBuilder.Entity<UserMedia>()
        .UseTphMappingStrategy()            // TPH by default but only for explicitness
        .HasOne(um => um.UserProfile);

        // UserMovie - Movie relation
        modelBuilder.Entity<UserMovie>()
        .HasOne(m => m.Movie);

        // UserSeries - Series relation
        modelBuilder.Entity<UserSeries>()
        .HasOne(s => s.Series);

        // UserSeason - Season relation
        modelBuilder.Entity<UserSeason>()
        .HasOne(s => s.Season);
    } 


}