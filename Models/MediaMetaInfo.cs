
namespace TvTracker.Models;

/// <summary>
/// Encapsulation of media meta info shared between different media types.
/// </summary>
public class MediaMetaInfo
{
    public string Title{get;}
    public string Language{get;}
    public string? PosterPath{get;set;}

    /// <summary>
    /// Genres associated with the media type e.g., comedy, romance, etc.
    /// </summary>
    public HashSet<Enums.Genre> Genres{get;private set;} = [];

    public void AddGenre(Enums.Genre genre) => Genres.Add(genre);

    public MediaMetaInfo(string title, string posterPath, string language)
    {
        ArgumentException.ThrowIfNullOrEmpty(title);
        Title = title;

        PosterPath = posterPath;

        ArgumentException.ThrowIfNullOrEmpty(language);
        Language = language;
    }
}

