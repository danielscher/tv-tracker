


using TvTracker.Models.Enums;

namespace TvTracker.Models;

/// <summary>
/// Represents a relationship between a user and a concrete media,
/// and holds user-specific meta data about the media.
/// </summary>
/// <param name="profile"> the associated user profile </param>
/// <param name="type"> either a movie, series or a season</param>
/// <param name="mediaId"> the id of the associated media </param>
/// <param name="status"> user status regarding the media </param>
public class UserMedia(in Profile profile, in MediaType type, in int mediaId, WatchStatus status = WatchStatus.None)
{
    public int Id{get;private set;}

    public Profile UserProfile{get;private set;} = profile;

    public MediaType MediaType{get;private set;} = type;

    public int MediaId{get;private set;} = mediaId;

    public WatchStatus Status{get;set;} = status;

    public int? Rating{get;set;}
}