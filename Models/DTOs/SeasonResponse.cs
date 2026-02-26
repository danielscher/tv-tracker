using System.Text.Json.Serialization;

namespace TvTracker.Models.DTOs;
public record SeasonResponse(
    [property: JsonPropertyName("id")] int Id,

    [property: JsonPropertyName("name")] string Title,
    [property: JsonPropertyName("overview")] string Description,
    [property: JsonPropertyName("poster_path")] string PosterPath,
    [property: JsonPropertyName("air_date")] string? ReleaseDate,
    [property: JsonPropertyName("season_number")] int SeasonNumber,
    [property: JsonPropertyName("episode_count")] int EpisodeCount
);
