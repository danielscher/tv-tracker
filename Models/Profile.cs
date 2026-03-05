namespace TvTracker.Models;

/// <summary>
/// Represents a local user profile, and enables multiple users per app instance.
/// </summary>
public class Profile(string name, string avatarPath)
{
    public int Id {get;}
    public string Name{get;set;} = name.Trim();
    public string AvatarPath = avatarPath;
}