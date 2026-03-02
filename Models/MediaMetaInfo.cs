
namespace TvTracker.Models;

/// <summary>
/// Encapsulation of media meta info shared between different media types.
/// </summary>
public class MediaMetaInfo
{
    public string Title{get;}
    public string Language{get;}
    public string? PosterPath{get;set;}
    public DateTime? ReleaseDate {get;set;}

    /// <summary>
    /// Genres associated with the media type e.g., comedy, romance, etc.
    /// </summary>
    public List<Enums.Genre> Genres{get;private set;} = [];

    public void AddGenre(Enums.Genre genre)
    {
        if (!Genres.Contains(genre))
            Genres.Add(genre);
    }

    public MediaMetaInfo(string title, string? posterPath, string language, DateTime? releaseDate)
    {
        ArgumentException.ThrowIfNullOrEmpty(title);
        Title = title;

        PosterPath = posterPath;

        ArgumentException.ThrowIfNullOrEmpty(language);
        Language = language;
        ReleaseDate = releaseDate;
    }
}

