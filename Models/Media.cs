
namespace TvTracker.Models;

/// <summary>
/// Base class for media encapsulating meta data.
/// </summary>
/// <param name="title"> the name of the media</param>
/// <param name="posterPath"> path to the image of the media </param>
/// <param name="language"> the original language of the media </param>
public abstract class Media(in string title, in string posterPath, in string language)
{
    public int Id{get;private set;}

    public string Title{get;private set;} = title;
    public string Language{get;private set;} = language;

    public string PosterPath{get;set;} = posterPath;

    /// <summary>
    /// Genres associated with the media type e.g., comedy, romance, etc.
    /// </summary>
    public HashSet<Enums.Genre> Genres{get;private set;} = [];

    /// <summary>
    /// Actors played in the series
    /// </summary>
    private readonly ICollection<CastMember> _cast = [];

    /// <summary>
    /// Cast ordered by the appearance in credits.
    /// </summary>
    public IEnumerable<CastMember> Cast => _cast.OrderBy(member => member.CreditIndex);

    public void AddCastMember(CastMember member)
    {
        _cast.Add(member);
    }
}

