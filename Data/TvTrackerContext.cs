using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TvTracker.Exception;
using TvTracker.Models;

namespace TvTracker.Data;

public class TvTrackerContext(DbContextOptions<TvTrackerContext> options) : DbContext(options)
{
    public DbSet<Profile> Profiles {get; set;}
    public DbSet<UserMedia> UserMedia{get;set;}
    public DbSet<Actor> Actors {get;set;}
    public DbSet<CastMember> Cast {get;set;}



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureProfiles(modelBuilder.Entity<Profile>());

        ConfigureMediaTables(modelBuilder);

        ConfigureUserMedia(modelBuilder);

        ConfigureActorTable(modelBuilder.Entity<Actor>());

        ConfigureCastMembers(modelBuilder.Entity<CastMember>());
    }

    /// <summary>
    /// Configures the UserMedia Hierarchy relationships
    /// </summary>
    private void ConfigureUserMedia(ModelBuilder modelBuilder)
    {

        // UserMedia - profile relation
        modelBuilder.Entity<UserMedia>
        (entity =>
            {   
                entity.Property(x => x.Id);
                entity.UseTphMappingStrategy(); // tph by default.
                entity.HasOne(x => x.Profile);
            }
        );

        // UserMovie - Movie relation
        modelBuilder.Entity<UserMovie>(e =>
        {
            e.HasOne(x => x.Movie);
        });

        // UserSeries - Series relation
        modelBuilder.Entity<UserSeries>(e =>
        {
            e.HasOne(x => x.Series);
        });

        // UserSeason - Season relation
        modelBuilder.Entity<UserSeason>(e =>
        {
            e.HasOne(x => x.Season);
        });
    } 

    private void ConfigureMediaTables(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Media>(entity =>
        {
            entity.Property(x=>x.Id);
            // flatten base type onto separate subclass tables.
            entity.UseTpcMappingStrategy();

            // configure owned type props.
            entity.ComplexProperty(x=> x.MediaInfo, m=> 
            {
                m.Property(x=>x.Title).HasMaxLength(100).IsRequired();
                m.Property(x=>x.Language).HasMaxLength(10).IsRequired();
            });

            // cast relation.
            entity.HasMany(x=>x.Cast)
            .WithOne()
            .HasForeignKey(x=> x.MediaId)
            .IsRequired();
        });

        modelBuilder.Entity<Movie>()
        .ToTable("Movies")
        .Property(x=> x.Length);

        modelBuilder.Entity<Movie>()
        .Property(x=>x.ReleaseYear);

        modelBuilder.Entity<Series>()
        .ToTable("Series")
        .HasMany(x=>x.Seasons)
        .WithOne()
        .HasForeignKey(x => x.SeriesId);

        modelBuilder.Entity<Series>()
        .Property(x=> x.AirStatus);

        modelBuilder.Entity<Season>(entity =>
        {
            entity.ToTable("Seasons");
            entity.Property(x => x.Id);
            entity.Property(x => x.SeriesId);
            entity.Property(x => x.SeasonNumber).IsRequired();
            entity.Property(x => x.Episodes).IsRequired();
            entity.Property(x=> x.EpisodeLength);
        });
    }

    private void ConfigureActorTable(EntityTypeBuilder<Actor> builder)
    {
        builder.ToTable("Actors");
        builder.Property(x => x.Id);
        builder.Property(x => x.FullName);
    }

    private void ConfigureCastMembers(EntityTypeBuilder<CastMember> builder)
    {
        builder.ToTable("CastMembers")
        .Property(x=>x.MediaId); 

        builder.Property(x => x.Id);
        builder.Property(x => x.MediaId);
        builder.Property(x => x.CharacterName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.CreditIndex).IsRequired();

        builder.HasOne(x=> x.Actor)
        .WithMany()
        .IsRequired();
    }

    private void ConfigureProfiles(EntityTypeBuilder<Profile> builder)
    {
        builder.Property(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(20);
    }
}