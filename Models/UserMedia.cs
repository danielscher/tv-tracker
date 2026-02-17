


using TvTracker.Models.Enums;

namespace TvTracker.Models;

/// <summary>
/// Represents a relationship between a user and a concrete media,
/// and holds user-specific rating and watch status.
/// </summary>
/// <param name="profile"> the associated user profile </param>
/// <param name="type"> either a movie, series or a season</param>
/// <param name="mediaId"> the id of the associated media </param>
/// <param name="status"> user status regarding the media </param>
public abstract class UserMedia
{
    public int Id {get;}

    public Profile Profile {get;}

    public WatchStatus Status{get;private set;} = WatchStatus.None;

    public int? Rating{get;set;}

    /// <summary>
    /// the UTC time when the 
    /// </summary>
    public DateTime? WatchedAt{get;private set;} 

    /// <summary>
    /// Sets the timestamp of the watched time to the current UTC time.
    /// </summary>
    public void Watch()
    {
        Status = WatchStatus.Watched;
        WatchedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets status to WantToWatch. Does not overwrite WatchedAt.
    /// </summary>
    public void WantToWatch()
    {
        Status = WatchStatus.WantToWatch;   
    }

    protected UserMedia(Profile profile)
    {
        Profile = profile;
    }

    protected UserMedia()
    {
        Profile = null!;
    }
}