using System.Text.Json.Serialization;

namespace TvTracker.Models.DTOs;
public record SeasonDetailsResponse(
    [property: JsonPropertyName("season_number")] int SeasonNumber,
    [property: JsonPropertyName("episodes")] ICollection<EpisodeResponse> Episodes
);
