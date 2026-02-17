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
    
    /// <summary>
    /// Fetches the specified UserMedia.
    /// Useful if only UserMedia related properties are needed.
    /// </summary>
    /// <param name="profileId"> id of data owner </param>
    /// <param name="mediaId"> id of desired UserMedia </param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<UserMedia> GetUserMedia(int profileId, int mediaId) 
    {
        var um =  await _context.UserMedia.Where(u => u.Profile.Id == profileId)
        .Where(u=> u.Id == mediaId)
        .SingleOrDefaultAsync(); 

        return um ?? throw new NotFoundException($"The media {mediaId} for profile {profileId} not found.");
    }

    public async Task RemoveAllUserMedia(int profileId)
    {
       await _context.UserMedia.Where(u=>u.Id == profileId).ExecuteDeleteAsync();
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

    public async Task RateUserMedia(int profileId, int userMediaId ,int rating)
    {
        var um = await GetUserMedia(profileId,userMediaId);
        um.Rating = rating;
        await _context.SaveChangesAsync();
    }


    public async Task UpdateWatchStatus<T>(
        int profileId, 
        int userMediaId, 
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
    }

    public Task<List<UserMovie>> GetUserMovies(int profileId) => GetUserMediaList<UserMovie>(profileId);
    public Task<List<UserSeries>> GetUserSeries(int profileId) => GetUserMediaList<UserSeries>(profileId);
    public Task<List<UserSeason>> GetUserSeasons(int profileId) => GetUserMediaList<UserSeason>(profileId);
    public Task<List<UserMovie>> GetUserMovieWatchList(int profileId) => GetUserMediaList<UserMovie>(profileId,WatchStatus.WantToWatch);
    public Task<List<UserSeries>> GetUserSeriesWatchList(int profileId) => GetUserMediaList<UserSeries>(profileId,WatchStatus.WantToWatch);
    public Task<UserMovie> GetUserMovie(int profileId,int userMediaId) => GetUserMedia<UserMovie>(profileId,userMediaId);
    public Task<UserSeries> GetUserSeries(int profileId,int userMediaId) => GetUserMedia<UserSeries>(profileId,userMediaId);
    public Task<UserSeason> GetUserSeason(int profileId,int userMediaId) => GetUserMedia<UserSeason>(profileId,userMediaId);

    public async Task<ICollection<UserMediaView>> GetRecentlyWatchedMedia(int profileId)
    {
        var media = await _context.UserMedia
        .Where(u => u.Profile.Id == profileId && u.Status == WatchStatus.Watched)
        .OrderBy(u => u.WatchedAt)
        .ToListAsync();

        return [.. media.Select(MapToView)];
    }


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
    /// <param name="userMediaId"> the user media id </param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    private async Task<T> GetUserMedia<T>(int profileId, int userMediaId) where T: UserMedia
    {
        var userMedia =  await _context.UserMedia.OfType<T>()
        .Where(u => u.Profile.Id == profileId)
        .Where(u=> u.Id == userMediaId)
        .SingleOrDefaultAsync(); 
        return userMedia ?? throw new NotFoundException($"The media {userMediaId} for profile {profileId} not found.");
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
    private UserMediaView MapToView(UserMedia u)
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

        return new UserMediaView(u.Id, type, title, poster, u.Rating, u.WatchedAt);
    }

}
