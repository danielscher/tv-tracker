using TvTracker.Models.DTOs;

namespace TvTracker.Models;
public class Movie : Media
{

    /// <summary>
    /// Movie length in minutes.
    /// </summary>
    public int Length{get;}

    public int ReleaseYear{get;}

    public Movie(int tmdbId, MediaMetaInfo mediaInfo, int length, int releaseYear) : base(tmdbId,mediaInfo)
    {
        Length = length;
        ReleaseYear = releaseYear;
    }

    // EF materialization.
    private Movie(int length, int releaseYear) : base()
    {
        Length = length;
        ReleaseYear = releaseYear;
    }

    public override MediaView ToResponse()
    {
        return new MediaView(Id, TmdbId, Enums.MediaType.Movie, MediaInfo.Title,MediaInfo.PosterPath,null,null);
    }

    public static Movie CreateMovie(MovieDetailsResponse dto,Func<string,string> imageUrlBuilder) 
    {   
        var imageUrl = dto.PosterPath != null ? imageUrlBuilder(dto.PosterPath):null;
        var info = new MediaMetaInfo(dto.Title,imageUrl,dto.Language);
        return new Movie(dto.Id,info,dto.Runtime,int.Parse(dto.ReleaseDate[..4]));
    }
}
