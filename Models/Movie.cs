using TvTracker.Models.DTOs;
using TvTracker.Models.View;

namespace TvTracker.Models;
public class Movie : Media
{

    /// <summary>
    /// Movie length in minutes.
    /// </summary>
    public int Length{get;}


    public Movie(int tmdbId, MediaMetaInfo mediaInfo, int length) : base(tmdbId,mediaInfo)
    {
        Length = length;
    }

    // EF materialization.
    private Movie(int length) : base()
    {
        Length = length;
    }

    public static Movie CreateMovie(MovieDetailsResponse dto,Func<string,string> imageUrlBuilder) 
    {   
        var imageUrl = dto.PosterPath != null ? imageUrlBuilder(dto.PosterPath):null;
        DateTime? releaseDate = dto.ReleaseDate != null ? DateTime.Parse(dto.ReleaseDate) : default(DateTime?);
        var info = new MediaMetaInfo(dto.Title,imageUrl,dto.Language,releaseDate);
        return new Movie(dto.Id,info,dto.Runtime);
    }
}
