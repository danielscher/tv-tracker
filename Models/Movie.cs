namespace TvTracker.Models;
public class Movie(in string title, in string posterPath, in string language) : Media(title, posterPath, language)
{
    /// <summary>
    /// Movie length in minutes.
    /// </summary>
    public int Length{get;set;}
    public int ReleaseYear{get;set;}
}
