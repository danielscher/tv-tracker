namespace TvTracker.Models;

/// <summary>
/// Represents an actor.
/// </summary>
public class Actor
{
    public int Id{get;}
    public string FullName{get;}
    public string? PosterPath{get;set;}

    public Actor(string fullName, string posterPath)
    {
        if (string.IsNullOrEmpty(fullName)) throw new ArgumentNullException(nameof(fullName));
        FullName = fullName.Trim();
        PosterPath = posterPath;
    }

    // EF materialization.
    private Actor(int id, string fullName, string posterPath)
    {
        Id = id;
        FullName = fullName;
        PosterPath = posterPath;
    }
}