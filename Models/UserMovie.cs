namespace TvTracker.Models;

public class UserMovie : UserMedia
{
    public  Movie Movie {get;}

    public UserMovie(Profile profile, Movie movie): base(profile,movie.Id) => Movie = movie;

    private UserMovie() : base()
    {
        Movie = null!;
    }    
}