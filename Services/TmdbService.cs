using System.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;
using TvTracker.Exception;
using TvTracker.Models.DTOs;
using TvTracker.Models.View;

public class TmdbService
{
    private readonly HttpClient _client;
    private readonly TmdbOptions _options;

    private readonly string imageWidth = "w500"; // hardcoded for now.

    public TmdbService(HttpClient client, IOptions<TmdbOptions> options)
    {
        _client = client;
        _options = options.Value;
        _client.BaseAddress = new Uri(_options.BaseUrl);
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

    public async Task<MovieDetailsResponse?> getMovieDetails(int TmbdMovieId)
    {
        var movieURL = BuildUrl($"movie/{TmbdMovieId}", new ()
        {
            ["append_to_response"] = "credits"
        });

        var movieResponse = await _client.GetAsync(movieURL);
        movieResponse.EnsureSuccessStatusCode();

        var movieDetails = await movieResponse.Content
            .ReadFromJsonAsync<MovieDetailsResponse>();
        
        return movieDetails;
        
    }

    public async Task<SeriesDetailsResponse?> GetSeriesDetails(int tmdbSeriesId)
    {
        var movieURL = BuildUrl($"tv/{tmdbSeriesId}", new ()
        {
            ["append_to_response"] = "credits"
        });

        var response = await _client.GetAsync(movieURL);
        response.EnsureSuccessStatusCode();

        var seriesResponse = await response.Content
            .ReadFromJsonAsync<SeriesDetailsResponse>();
        
        return seriesResponse;
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


}