using System.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TvTracker.Exception;
using TvTracker.Models;
using TvTracker.Models.DTOs;
using TvTracker.Models.Enums;
using TvTracker.Models.View;

public class TmdbService
{
    private readonly HttpClient _client;
    private readonly TmdbOptions _options;
    private readonly IMemoryCache _cache;

    private readonly string imageWidth = "w500"; // hardcoded for now.

    public TmdbService(HttpClient client, IOptions<TmdbOptions> options, IMemoryCache cache)
    {
        _client = client;
        _options = options.Value;
        _client.BaseAddress = new Uri(_options.BaseUrl);
        _cache = cache;
    }

    public async Task<ICollection<SearchResponseView>> SearchMovies(string query)
    {
        var url = BuildUrl("search/movie",new()
        {
            ["query"] = query,
            ["language"] = "en-US",

        });

        var response = await _client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var data = await response.Content
            .ReadFromJsonAsync<SearchWrapperResponse<MovieSearchResponse>>();
        
        return data?.Results?
                .Select(x => new SearchResponseView
                {
                    TmdbId = x.TmdbId,
                    Title = x.Title,
                    PosterUrl = x.PosterPath != null ? PosterUrlBuilder(x.PosterPath) : null,
                })
                .ToList() ?? [];
    }

    public async Task<ICollection<SearchResponseView>> SearchSeries(string query)
    {
        var url = BuildUrl("search/tv",new()
        {
            ["query"] = query,
            ["language"] = "en-US",
        });
        var response = await _client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var data = await response.Content
            .ReadFromJsonAsync<SearchWrapperResponse<SeriesSearchResponse>>();
        
        var mapped = data?.Results?.Select(x => 
                new SearchResponseView 
                {
                    TmdbId = x.TmdbId,
                    Title = x.Title,
                    PosterUrl = x.PosterPath != null ? PosterUrlBuilder(x.PosterPath) : null,
                }).ToList() ?? [];
        return mapped;
    }

    public async Task<ICollection<SearchResponseView>> GetTrendingMovies()
    {
        var url =  BuildUrl("trending/movie/week", new ()
        {
            ["language"] = "en-US"
        });
        
        var cacheKey = "movie:trending";
        var result = await GetOrCreateCacheEntry(cacheKey, ()=> MakeRequestAndParse<SearchWrapperResponse<MovieSearchResponse>>(url),60);
        return result?.Results.Select(x => 
            new SearchResponseView
            {
                TmdbId = x.TmdbId,
                Title= x.Title,
                PosterUrl = x.PosterPath != null ? PosterUrlBuilder(x.PosterPath) : null,
            }
            ).ToList() ?? [];
    }

    public async Task<ICollection<SearchResponseView>> GetUpcomingMovies()
    {
        var url =  BuildUrl("movie/upcoming", new ()
        {
            ["language"] = "en-US",
            ["region"] = "DE"
        });
        
        var cacheKey = "movie:upcoming";
        var result = await GetOrCreateCacheEntry(cacheKey, ()=> MakeRequestAndParse<SearchWrapperResponse<MovieSearchResponse>>(url),60);
        return result?.Results.Where(x=> x.ReleaseDate != null ? DateTime.Parse(x.ReleaseDate) > DateTime.Today : false ).Select(x => 
            new SearchResponseView
            {
                TmdbId = x.TmdbId,
                Title= x.Title,
                PosterUrl = x.PosterPath != null ? PosterUrlBuilder(x.PosterPath) : null,
            }
            ).ToList() ?? [];
    }

    public async Task<ICollection<SearchResponseView>> GetInTheaters()
    {
        var url =  BuildUrl("movie/now_playing", new ()
        {
            ["language"] = "en-US"
        });
        
        var cacheKey = "movie:in_theaters";
        var result = await GetOrCreateCacheEntry(cacheKey, ()=> MakeRequestAndParse<SearchWrapperResponse<MovieSearchResponse>>(url),60);
        return result?.Results.Select(x => 
            new SearchResponseView
            {
                TmdbId = x.TmdbId,
                Title= x.Title,
                PosterUrl = x.PosterPath != null ? PosterUrlBuilder(x.PosterPath) : null,
            }
            ).ToList() ?? [];
    }

    public async Task<ICollection<SearchResponseView>> GetTrendingSeries()
    {
        var url =  BuildUrl("trending/tv/week", new ()
        {
            ["language"] = "en-US"
        });
        
        var cacheKey = "tv:trending";
        var result = await GetOrCreateCacheEntry(cacheKey, ()=> MakeRequestAndParse<SearchWrapperResponse<SeriesSearchResponse>>(url),60);
        return result?.Results.Select(x => 
            new SearchResponseView
            {
                TmdbId = x.TmdbId,
                Title= x.Title,
                PosterUrl = x.PosterPath != null ? PosterUrlBuilder(x.PosterPath) : null,
            }
            ).ToList() ?? [];
    }

        public async Task<ICollection<SearchResponseView>> GetUpcomingEpisodes()
    {
        var url =  BuildUrl("tv/on_the_air", new ()
        {
            ["language"] = "en-US"
        });
        
        var cacheKey = "tv:upcoming";
        var result = await GetOrCreateCacheEntry(cacheKey, ()=> MakeRequestAndParse<SearchWrapperResponse<SeriesSearchResponse>>(url),60);
        return result?.Results.Select(x => 
            new SearchResponseView
            {
                TmdbId = x.TmdbId,
                Title= x.Title,
                PosterUrl = x.PosterPath != null ? PosterUrlBuilder(x.PosterPath) : null,
            }
            ).ToList() ?? [];
    }

    public async Task<MovieDetailsResponse?> GetMovieDetails(int TmdbMovieId)
    {
        var movieURL = BuildUrl($"movie/{TmdbMovieId}", new ()
        {
            ["append_to_response"] = "credits"
        });

        var cacheKey = $"movie:details:{TmdbMovieId}";
        return await GetOrCreateCacheEntry(cacheKey,()=> MakeRequestAndParse<MovieDetailsResponse>(movieURL),20);
    }

    public async Task<SeriesDetailsResponse?> GetSeriesDetails(int tmdbSeriesId)
    {
        var movieURL = BuildUrl($"tv/{tmdbSeriesId}", new ()
        {
            ["append_to_response"] = "credits"
        });

        var cacheKey = $"series:details:{tmdbSeriesId}";
        return await GetOrCreateCacheEntry(cacheKey,()=> MakeRequestAndParse<SeriesDetailsResponse>(movieURL),20);
    }

    /// <summary>
    /// performs request to fetch season details of series and aggregate the episode runtime.
    /// each request is performed in parallel.
    /// </summary>
    /// <param name="seasonCount"> the total number of seasons (including specials) </param>
    /// <returns></returns>
    public async Task<Dictionary<int,int>> GetSeriesSeasonRuntime(int tmdbSeriesId,int seasonCount)
    {
        Dictionary<int,int> seasonToRuntime = [];

        // skip specials (start from seasonNum = 1)
        var tasks = Enumerable.Range(1, seasonCount).Select( async seasonNumber =>
        {
            var seasonUrl = BuildUrl($"tv/{tmdbSeriesId}/season/{seasonNumber}", new() {});
            var seasonResponse = await MakeRequestAndParse<SeasonDetailsResponse>(seasonUrl);
            var runtime = seasonResponse?.Episodes.Sum(x => x.Runtime);
            return (SeasonNumber : seasonNumber, Runtime: runtime ?? 0);
        }).ToArray();
        
        var results = await Task.WhenAll(tasks);
        return results.ToDictionary(x=> x.SeasonNumber, x=>x.Runtime);
    }

    public async Task<ICollection<CollectionEntry>> GetMovieCollection(int tmdbCollectionId)
    {
        var collectionsUrl = BuildUrl($"collection/{tmdbCollectionId}", new (){});

        var cacheKey = $"collections:{tmdbCollectionId}";
        var collection = await GetOrCreateCacheEntry(cacheKey, () => MakeRequestAndParse<CollectionResponse>(collectionsUrl),20) 
        ?? throw new NotFoundException($"Failed to find Collection with id {tmdbCollectionId}");

        return collection.Entries;
    }

    private string BuildUrl(string path, Dictionary<string, string?> queryParams)
    {
        queryParams["api_key"] = _options.ApiKey;

        return QueryHelpers.AddQueryString(path, queryParams);
    }

    public string PosterUrlBuilder(string posterPath)
    {
        return $"{_options.ImageBaseUrl}\\{imageWidth}\\{posterPath}";
    }


    private async Task<T?> MakeRequestAndParse<T>(string url)
    {
        Console.WriteLine(url);
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    private async Task<T?> GetOrCreateCacheEntry<T>(
    string cacheKey,
    Func<Task<T?>> factory,
    int expirationInMinutes = 60)
    {
        if (_cache.TryGetValue(cacheKey, out T? cached))
        {
            return cached;
        }

        var result = await factory();

        if (result != null) // don't cache nulls
        {
            _cache.Set(cacheKey, result,
                TimeSpan.FromMinutes(expirationInMinutes));
        }

        return result;
    }
}