using TvTracker.Models.Enums;

namespace TvTracker.Models;

public class UserMovie(Profile profile, Movie movie) : UserMedia(profile)
{
    public Movie Movie = movie;
    
}