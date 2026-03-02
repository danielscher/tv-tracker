using TvTracker.Models.View;

namespace TvTracker.Models;

public class UserMovie : UserMedia
{
    public  Movie Movie {get;}

    public UserMovie(Profile profile, Movie movie): base(profile,movie.Id ,movie.TmdbId) => Movie = movie;

    private UserMovie() : base()
    {
        Movie = null!;
    }  

    public override MediaView ToView()
    {
        return new (
            Movie.TmdbId,
            Enums.MediaType.Movie,
            Movie.MediaInfo.Title,
            Movie.MediaInfo.PosterPath,
            Movie.MediaInfo.ReleaseDate,
            Rating,
            Status,
            WatchedAt
        );
    }
}