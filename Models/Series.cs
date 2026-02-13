using TvTracker.Models.Enums;

namespace TvTracker.Models;

/// <summary>
/// Represents a Tv Show.
/// </summary>
/// <param name="title"> name of the series </param>
/// <param name="posterPath"> path to the image of the series </param>
/// <param name="language"> the original language of the series </param>
/// <param name="status"> current airing status </param>
public class Series(in string title, in string posterPath, in string language, in AirStatus status)
{
    public MediaMetaInfo MetaInfo = new(title, posterPath, language);
    public AirStatus AirStatus{get;set;} = status;
    private readonly ICollection<Season> _seasons = [];
    /// <summary>
    /// orders seasons by their release/season number.
    /// </summary>
    public IEnumerable<Season> Seasons => _seasons.OrderBy(s => s.SeasonNumber);

}