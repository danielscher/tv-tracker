using System.Text.Json.Serialization;

namespace TvTracker.Models.DTOs;

/// <summary>
/// Record for the deserialization of the JSON obtained
/// from TMDB /search/tv endpoint 
/// </summary>
public record SeriesSearchResponse(
    [property: JsonPropertyName("id")] int TmdbId,
    [property: JsonPropertyName("original_name")] string? Title,
    [property: JsonPropertyName("poster_path")] string? PosterPath
);



