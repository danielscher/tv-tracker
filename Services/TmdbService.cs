using System.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Query;
using TvTracker.Exception;
using TvTracker.Models.DTOs;
using TvTracker.Models.View;

public class TmbdService
{
    private readonly HttpClient _client;
    private readonly string _apiKey;

    public TmbdService(HttpClient client, IConfiguration config)
    {
        _client = client;
        _apiKey = config.GetValue<string>("Tmdb:ApiKey") ?? throw new ConfigurationException("TMBD API key not found.");
        Console.WriteLine(_apiKey);
    }

    public async Task Test()
    {
        var url = BuildUrl("movie/1368166", new()
        {
        });
        var response = await _client.GetAsync(url);
        var body = await response.Content.ReadAsStringAsync();
        Console.WriteLine(body);
    }

    public async Task<ICollection<SearchResponseView>> SearchMovies(string query)
    {
        var url = BuildUrl("search/movie",new()
        {
            ["query"] = query,
            ["language"] = "en-US",

        });

        Console.WriteLine($" baseURL: {_client.BaseAddress}");
        Console.WriteLine($"URL: {url}");

        var response = await _client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var data = await response.Content
            .ReadFromJsonAsync<SearchWrapperResponse<MovieSearchResponse>>();
        
        return data?.Results?
                .Select(x => new SearchResponseView
                {
                    TmdbId = x.TmdbId,
                    Title = x.Title,
                    PosterPath = x.PosterPath
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
                    PosterPath = x.PosterPath
                }).ToList() ?? [];
        return mapped;
    }

    public async Task<MovieDetailsResponse?> getMovieDetails(int TmbdMovieId)
    {
        var movieURL = BuildUrl($"movie/{TmbdMovieId}", new ()
        {
            ["append_to_response"] = "credits"
        });


        Console.WriteLine($" baseURL: {_client.BaseAddress}");
        Console.WriteLine($"URL: {movieURL}");

        var movieResponse = await _client.GetAsync(movieURL);
        movieResponse.EnsureSuccessStatusCode();

        var movieDetails = await movieResponse.Content
            .ReadFromJsonAsync<MovieDetailsResponse>();
        
        return movieDetails;
        
    }

    private string BuildUrl(string path, Dictionary<string, string?> queryParams)
    {
        queryParams["api_key"] = _apiKey;

        return QueryHelpers.AddQueryString(path, queryParams);
    }
}