namespace TvTracker.Models;

/// <summary>
/// Represents an actor.
/// </summary>
/// <param name="fullName"> name of the actor </param>
/// <param name="posterPath"> path to the actors image </param>
public class Actor(string fullName, in string posterPath)
{
    public int Id{get;private set;}
    public string FullName{get;set;} = fullName.Trim();
    public string posterPath{get;set;} = posterPath;
}