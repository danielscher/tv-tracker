using System.Text.Json.Serialization;

namespace TvTracker.Models.View;

/// <summary>
/// View model for presenting search response of media.
/// </summary>
public record SearchResponseView
{
    [JsonPropertyName("TmdbId")]
    public int TmdbId { get; init; }
    [JsonPropertyName("Title")]
    public string? Title { get; init; }
    [JsonPropertyName("PosterPath")]
    public string? PosterPath { get; init; }
}



