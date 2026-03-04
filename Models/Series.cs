using TvTracker.Models.DTOs;
using TvTracker.Models.Enums;

namespace TvTracker.Models;

/// <summary>
/// Represents a Tv Show.
/// </summary>
public class Series : Media
{
    public AirStatus AirStatus{get;set;}

    private readonly ICollection<Season> _seasons = [];

    public IEnumerable<Season> Seasons => _seasons;

    public Series(int tmdbId, MediaMetaInfo mediaInfo, AirStatus airStatus) : base(tmdbId,mediaInfo)
    {
        AirStatus = airStatus;
    }

    // EF materialization.
    private Series(AirStatus airStatus) : base()
    {
        AirStatus = airStatus;
    }

    public void AddSeason(Season season)
    {
        _seasons.Add(season);
    }

    public void AddSeasonRange(ICollection<Season> seasons)
    {
        foreach (var s in seasons)
        {
            AddSeason(s);
        }
    }

    public static Series Create(SeriesDetailsResponse dto, Func<string,string> imageUrlBuilder,Dictionary<int,int> seasonRunTimes) 
    {   
        var imageUrl = dto.PosterPath != null ? imageUrlBuilder(dto.PosterPath) : null;
        DateTime? releaseDate = dto.ReleaseDate != null ? DateTime.Parse(dto.ReleaseDate) : default(DateTime?);
        var info = new MediaMetaInfo(dto.Title,imageUrl,dto.Language,releaseDate);
        var status = StatusMapper.ToDomain(dto.Status);
        var series = new Series(dto.Id,info,status);
        foreach (var seasonResponse in dto.Seasons)
        {
            var runtime = seasonRunTimes.GetValueOrDefault(seasonResponse.SeasonNumber);
            var season = Season.Create(seasonResponse,runtime);
            Console.WriteLine(season.EpisodeLength);
            series.AddSeason(season);
        }
        return series;
    }
}