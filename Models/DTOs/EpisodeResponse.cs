using System.Text.Json.Serialization;

public record EpisodeResponse(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("runtime")] int? Runtime
);