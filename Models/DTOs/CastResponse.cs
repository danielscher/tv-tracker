using System.Text.Json.Serialization;

public record CastMemberResponse(
    [property: JsonPropertyName("id")] int ActorId,
    [property: JsonPropertyName("name")] string ActorName,
    [property: JsonPropertyName("character")] string CharacterName,
    [property: JsonPropertyName("order")] int CreditsIndex,
    [property: JsonPropertyName("profile_path")] string PosterPath  
);