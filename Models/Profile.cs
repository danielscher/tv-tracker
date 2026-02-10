namespace TvTracker.Models;

/// <summary>
/// Represents a local user profile, and enables multiple users per app instance.
/// </summary>
public class Profile (string name)
{
    public int Id {get;private set;}
    public string Name{get;set;} = name;
}