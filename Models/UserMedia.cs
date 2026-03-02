


using TvTracker.Models.DTOs;
using TvTracker.Models.Enums;
using TvTracker.Models.View;

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
    public Guid Id {get;}

    public int ProfileId {get;}
    public Profile Profile {get;}

    /// <summary>
    /// Internal application media id.
    /// </summary>
    public Guid MediaId {get;protected set;}

    /// <summary>
    /// External TMDB API media id.
    /// </summary>
    public int TmdbId {get; protected set;}

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
    /// Toggles status between wantToWatch To None if not already watched.
    /// otherwise does nothing.
    /// </summary>
    public void WantToWatch()
    {
        if (Status == WatchStatus.Watched) {return;}
        Status = Status == WatchStatus.WantToWatch ? WatchStatus.None : WatchStatus.WantToWatch;   
    }

    protected UserMedia(Profile profile, Guid mediaId, int tmdbId)
    {
        Profile = profile;
        ProfileId = profile.Id;
        MediaId = mediaId;
        TmdbId = tmdbId;
    }

    protected UserMedia()
    {
        Profile = null!;
    }

    public UserMediaInfo Flatten()
    {
        return new UserMediaInfo(Id,Rating,Status,WatchedAt);
    }

    public abstract MediaView ToView();
}