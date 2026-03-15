using System.Text.Json.Serialization;

namespace TvTracker.Models.DTOs;

/// <summary>
/// Record for the deserialization of the JSON obtained
/// from TMDB /search/movie endpoint 
/// </summary>
public record MovieSearchResponse(
    [property: JsonPropertyName("id")] int TmdbId,
    [property: JsonPropertyName("original_title")] string? Title,
    [property: JsonPropertyName("poster_path")] string? PosterPath,
    [property: JsonPropertyName("release_date")] string? ReleaseDate
);
