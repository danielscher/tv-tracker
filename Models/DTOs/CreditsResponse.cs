using System.Text.Json.Serialization;

public record CreditsResponse (
    [property: JsonPropertyName("cast")] List<CastMemberResponse> Cast
);