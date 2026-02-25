using TvTracker.Data;
using TvTracker.Exception;
using TvTracker.Models;
using TvTracker.Models.Enums;
using Microsoft.EntityFrameworkCore;
using TvTracker.Models.DTOs;


namespace TvTracker.Services;

public class UserMediaService(TvTrackerContext context)
{
    private readonly TvTrackerContext _context = context;

    public async Task<UserMedia> getUserMediaByIdAndProfileId(Guid id,int profileId)
    {
       return await _context.UserMedia
       .Where(x=> x.Id == id && x.ProfileId == profileId)
       .SingleAsync() ?? throw new NotFoundException($"UserMedia with id {id} for profile {profileId} not found.");
    }
    
    /// <summary>
    /// Fetches the specified UserMedia.
    /// Useful if only UserMedia related properties are needed.
    /// </summary>
    /// <param name="profileId"> id of data owner </param>
    /// <param name="mediaId"> id of the underlying media </param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<UserMedia?> GetUserMediaByProfileIdAndMediaIdOptional(int profileId, Guid mediaId) 
    {
        var um =  await _context.UserMedia
        .Where(u => u.Profile.Id == profileId)
        .Where(u=> u.MediaId == mediaId)
        .SingleOrDefaultAsync(); 
        return um;
    }

    public async Task<UserMedia> GetUserMediaByProfileIdAndMediaId(int profileId, Guid mediaId)
    {
        return (await GetUserMediaByProfileIdAndMediaIdOptional(profileId,mediaId)) 
        ?? throw new NotFoundException($"The media {mediaId} for profile {profileId} not found."); 
    }

    public async Task<UserMedia?> GetUserMediaByProfileIdAndTmdbIdOptional(int profileId, int tmdbId) 
    {
        var um =  await _context.UserMedia
        .Where(u => u.Profile.Id == profileId)
        .Where(u=> u.TmdbId == tmdbId)
        .SingleOrDefaultAsync(); 
        return um;
    }

    public async Task<UserMedia> GetUserMediaByProfileIdAndTmdbId(int profileId, int tmdbId) 
    {
        var um =  await GetUserMediaByProfileIdAndTmdbIdOptional(profileId,tmdbId);
        return um ?? throw new NotFoundException($"The media with tmdbId:{tmdbId} for profile {profileId} not found");
    }


    public async Task RemoveAllUserMedia(int profileId)
    {
       await _context.UserMedia.Where(u=>u.Profile.Id == profileId).ExecuteDeleteAsync();
    }

    public async Task<UserMovie> CreateUserMovie(Profile profile, Movie movie)
    {
        UserMovie um = new(profile, movie);
        _context.UserMedia.Add(um);
        await _context.SaveChangesAsync();
        return um;
    }

    public async Task<UserSeries> CreateUserSeries(Profile profile, Series series)
    {
        UserSeries us = new(profile, series);
        _context.UserMedia.Add(us);
        await _context.SaveChangesAsync();
        return us;
    }

    public async Task<UserSeason> CreateUserSeason(Profile profile, Season season)
    {
        UserSeason us = new(profile, season);
        _context.UserMedia.Add(us);
        await _context.SaveChangesAsync();
        return us;
    }

    public async Task<UserMedia> RateUserMedia(int profileId, Guid userMediaId ,int rating)
    {
        Console.WriteLine($"new rating: {rating}");
        var um = await getUserMediaByIdAndProfileId(userMediaId,profileId);
        um.Rating = rating;
        await _context.SaveChangesAsync();
        return um;
    }


    /// <summary>
    /// Updates and persist UserMedia status.
    /// </summary>
    /// <typeparam name="T">derived type of UserMedia</typeparam>
    /// <param name="profileId">id of owning profile</param>
    /// <param name="userMediaId">id of the UserMedia</param>
    /// <param name="status">the new status</param>
    /// <returns>updated UserMedia</returns>
    public async Task<T> UpdateWatchStatus<T>(
        int profileId,
        Guid userMediaId, 
        WatchStatus status) where T : UserMedia
    {
        var um = await GetUserMedia<T>(profileId, userMediaId);

        switch (status)
        {
            case WatchStatus.WantToWatch : {um.WantToWatch(); break;}
            case WatchStatus.Watched : {um.Watch();break;}
            case WatchStatus.None : break;
        }
        await _context.SaveChangesAsync();
        return um;
    }

    public Task<List<UserMovie>> GetUserMovieWatchList(int profileId) => GetUserMediaList<UserMovie>(profileId,WatchStatus.WantToWatch);
    public Task<List<UserMovie>> GetUserMovieAlreadyWatchedList(int profileId) => GetUserMediaList<UserMovie>(profileId,WatchStatus.Watched);
    public Task<List<UserSeries>> GetUserSeriesWatchList(int profileId) => GetUserMediaList<UserSeries>(profileId,WatchStatus.WantToWatch);
    public Task<List<UserSeries>> GetUserSeriesAlreadyWatchedList(int profileId) => GetUserMediaList<UserSeries>(profileId,WatchStatus.Watched);

    /// <summary>
    /// Fetches the subclass of UserMedia Data of a certain profile. 
    /// </summary>
    /// <typeparam name="T">The derived type of UserMedia </typeparam>
    /// <param name="profileId"></param>
    /// <returns></returns>
    private Task<List<T>> GetUserMediaList<T>(int profileId ) where T: UserMedia
    {
        return _context.UserMedia.OfType<T>().Where(u => u.Profile.Id == profileId).ToListAsync();
    }

    /// <summary>
    /// Fetches the specified subclass with the provided status.
    /// </summary>
    /// <typeparam name="T">the desired subclass of UserMedia </typeparam>
    /// <param name="profileId"> data owner </param>
    /// <param name="status"></param>
    /// <returns></returns>
    private Task<List<T>> GetUserMediaList<T>(int profileId, WatchStatus status) where T: UserMedia
    {
        return _context.UserMedia.OfType<T>().Where(u => u.Profile.Id == profileId).Where(u => u.Status == status).ToListAsync();
    }

    /// <summary>
    /// Fetch a specific UserMedia subclass.
    /// </summary>
    /// <typeparam name="T">the desired subclass of UserMedia </typeparam>
    /// <param name="profileId">data owner id </param>
    /// <param name="userMediaId"> the UserMedia id </param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    private async Task<T> GetUserMedia<T>(int profileId, Guid userMediaId) where T: UserMedia
    {
        var userMedia =  await _context.UserMedia.OfType<T>()
        .Where(u => u.Profile.Id == profileId)
        .Where(u=> u.Id == userMediaId)
        .SingleOrDefaultAsync(); 
        return userMedia ?? throw new NotFoundException($"The userMedia {userMediaId} for profile {profileId} not found.");
    }

    /// <summary>
    /// Fetch the specified subclass that has been watched.
    /// </summary>
    /// <typeparam name="T"> the desired subclass</typeparam>
    /// <param name="profileId">data owner id</param>
    private Task<List<T>> GetRecentlyWatchUserMedia<T>(int profileId) where T: UserMedia
    {
        return _context.UserMedia.OfType<T>()
            .Where(u => u.Profile.Id == profileId)
            .Where(u => u.Status == Models.Enums.WatchStatus.Watched)
            .OrderBy(u => u.WatchedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Maps UserMedia to a dto version.
    /// </summary>
    /// <param name="u"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    /// <exception cref="NotImplementedException"></exception>
    public MediaView ToView(UserMedia u)
        {
        const string errorMsg = "UserSeason is not supported here.";

        string title = u switch
        {
            UserMovie m => m.Movie.MediaInfo.Title,
            UserSeries s => s.Series.MediaInfo.Title,
            UserSeason _ => throw new InvalidDataException(errorMsg),
            _ => throw new NotImplementedException()
        };

        string? poster = u switch
        {
            UserMovie m => m.Movie.MediaInfo.PosterPath,
            UserSeries s => s.Series.MediaInfo.PosterPath,
            UserSeason _ => throw new InvalidDataException(errorMsg),
            _ => throw new NotImplementedException()
        };

        MediaType type = u switch
        {
            UserMovie _ => MediaType.Movie,
            UserSeries _ => MediaType.Series,
            UserSeason _ => throw new InvalidDataException(errorMsg),
            _ => throw new NotImplementedException()
        };

        return new MediaView(u.MediaId, type, title, poster, u.Rating, u.Status);
    }

}
