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
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
                entity.UseTphMappingStrategy(); // tph by default.
                entity.HasOne(x => x.Profile);
                entity.HasIndex(um => new { um.ProfileId, um.MediaId }).IsUnique();
                entity.Property(x=>x.TmdbId).IsRequired();
            }
        );

        // UserMovie - Movie relation
        modelBuilder.Entity<UserMovie>(e =>
        {
            e.HasOne(x => x.Movie).WithMany().IsRequired();
            e.Navigation(x => x.Movie).AutoInclude();
        });

        // UserSeries - Series relation
        modelBuilder.Entity<UserSeries>(e =>
        {
            e.HasOne(x => x.Series).WithMany().IsRequired();
            e.Navigation(x=>x.Series).AutoInclude();
            
            e.HasMany(x => x.UserSeasons).WithOne();
            e.Navigation(x=> x.UserSeasons).AutoInclude();
        });

        // UserSeason - Season relation
        modelBuilder.Entity<UserSeason>(e =>
        {
            e.HasOne(x => x.Season).WithMany().IsRequired();
            e.Navigation(x=>x.Season).AutoInclude();
        });
    } 

    private void ConfigureMediaTables(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Media>(entity =>
        {
            entity.Property(x=>x.Id).ValueGeneratedOnAdd();
            // flatten base type onto separate subclass tables.
            entity.UseTpcMappingStrategy();

            // owned types must be configured
            // on tables of the derived classes

            // cast relation.
            entity.HasMany(x=>x.Cast)
            .WithOne()
            .HasForeignKey(x=> x.MediaId)
            .IsRequired();
            entity.Property(x=> x.TmdbId).IsRequired();
        });

        modelBuilder.Entity<Movie>(e =>
        {
            e.ToTable("Movies");
            e.Property(x=> x.Length);

            e.ComplexProperty(x=> x.MediaInfo, m=> 
            {
                m.Property(x=>x.Title).HasMaxLength(100).IsRequired();
                m.Property(x=>x.Language).HasMaxLength(10).IsRequired();
                m.Property(x=> x.ReleaseDate);
            });
            e.HasMany(x => x.Cast).WithOne().HasForeignKey(x => x.MediaId).IsRequired();
            e.Navigation(x=>x.Cast).AutoInclude();

        });

        modelBuilder.Entity<Series>(e =>
        {
            e.ToTable("Series")
                .HasMany(x=>x.Seasons)
                .WithOne()
                .HasForeignKey(x => x.SeriesId);

            e.ComplexProperty(x=> x.MediaInfo, m=> 
            {
                m.Property(x=>x.Title).HasMaxLength(100).IsRequired();
                m.Property(x=>x.Language).HasMaxLength(10).IsRequired();
                m.Property(x=> x.ReleaseDate);

            });

            e.Property(x=> x.AirStatus);
            e.Navigation(x => x.Seasons).AutoInclude(); 
            e.Navigation(x=>x.Cast).AutoInclude();
        });

        modelBuilder.Entity<Season>(entity =>
        {
            entity.ToTable("Seasons");
            entity.Property(x => x.Id);
            entity.Property(x => x.TmdbId).IsRequired();
            entity.Property(x => x.SeriesId);
            entity.Property(x => x.SeasonNumber).IsRequired();
            entity.Property(x => x.Episodes).IsRequired();
            entity.Property(x=> x.EpisodeLength);
            entity.Property(x => x.ReleaseDate);
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
        builder.ToTable("CastMembers");
        builder.Property(x => x.Id);
        builder.Property(x => x.MediaId);
        builder.Property(x => x.CharacterName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.CreditIndex).IsRequired();

        builder.HasOne(x=> x.Actor)
        .WithMany()
        .IsRequired();

        builder.Navigation(x=>x.Actor).AutoInclude();
    }

    private void ConfigureProfiles(EntityTypeBuilder<Profile> builder)
    {
        builder.Property(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(20);
        builder.Property(x=> x.AvatarPath);
    }

    public static void SeedData(DbContext context)
    {
        // Profiles
        var testProfile = new Profile("test","/assets/avatars/food.svg");
        context.Set<Profile>().Add(testProfile);

        // Actors
        var actor = new Actor("Robert Downey Jr.","posters/rdj.png");
        context.Set<Actor>().Add(actor);

        // Movies
        var movieInfo = new MediaMetaInfo("Iron Man", "posters/ironman.png", "en",DateTime.Now);
        var movie = new Movie(1726,movieInfo, 126);
        context.Set<Movie>().Add(movie);

        var movieInfo2 = new MediaMetaInfo("Iron Man 2", "posters/ironman.png", "en",DateTime.Now);
        var movie2 = new Movie(10138,movieInfo2, 124);
        context.Set<Movie>().Add(movie2);

        context.SaveChanges();

        // Cast
        var cast = new CastMember("Tony Stark", 0, actor);
        context.Set<CastMember>().Add(cast);
        movie.AddCast(cast);
        context.SaveChanges();

        var cast5 = new CastMember("Tony Stark", 0, actor);
        movie2.AddCast(cast5);
        context.SaveChanges();


        // UserMedia
        var userMovie = new UserMovie(testProfile,movie) {Rating = 6};
        userMovie.Watch();
        context.Set<UserMovie>().Add(userMovie);

        context.SaveChanges();

        // Series
        var seriesInfo = new MediaMetaInfo("Friends","posters/friends.png", "en",DateTime.Now);
        var series = new Series(1668,seriesInfo,Models.Enums.AirStatus.Finished);
        var season1 = new Season(1,24,22, DateTime.Now);
        var season2 = new Season(2,24,22, DateTime.Now);
        var season3 = new Season(3,25,22, DateTime.Now);
        series.AddSeasonRange([season1,season2,season3]);

        context.Set<Series>().Add(series);

        // Cast 
        var actor2 = new Actor("Jennifer Aniston","posters/jennifer_aniston.png");
        var actor3 = new Actor("David Schwimmer","posters/david_schwimmer.png");
        var actor4 = new Actor("Mathew Perry","posters/mathew_perry.png");
        context.Set<Actor>().AddRange([actor2,actor3,actor4]);

        var cast2 = new CastMember("Rachel Green", 0, actor2);
        var cast3 = new CastMember("Dr. Ross Geller", 1, actor3);
        var cast4 = new CastMember("Chandler Bing", 2, actor4);
        context.Set<CastMember>().AddRange([cast2,cast3,cast4]);

        series.AddCastRange([cast2,cast3,cast4]);

        context.SaveChanges();
    }
}