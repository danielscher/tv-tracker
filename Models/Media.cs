using TvTracker.Models.DTOs;

namespace TvTracker.Models;
public abstract class Media
{
    public Guid Id {get;}

    public int TmdbId {get;}
    
    /// <summary>
    /// Holds general info regarding the media.
    /// </summary>
    public MediaMetaInfo MediaInfo {get;}

    private readonly ICollection<CastMember> _cast = [];

    /// <summary>
    /// Cast ordered by the appearance in credits.
    /// </summary>
    public IEnumerable<CastMember> Cast => _cast;

    protected Media(int tmdbId, MediaMetaInfo mediaInfo) 
    {
        TmdbId = tmdbId;
        MediaInfo = mediaInfo;
    }

    // EF Materialization
    // cannot have id as param as ef will not map it during derived
    // class construction.
    protected Media()
    {
        MediaInfo = null!; // warning suppression.
    }

    public void AddCast(CastMember castMember)
    {
        if (!_cast.Contains(castMember))
        {
            _cast.Add(castMember);
        }
    }

    public void AddCastRange(ICollection<CastMember> members)
    {
        foreach (var member in members)
        {
            AddCast(member);
        }
    }
    
    /// <summary>
    /// Returns a flattened out version for UI.
    /// </summary>
    public abstract MediaView ToResponse();

}