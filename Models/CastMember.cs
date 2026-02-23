namespace TvTracker.Models;

/// <summary>
/// Represents a role or a character played by an actor in some media.
/// Essentially a join table between an Actor and a Media type.
/// </summary>
public class CastMember
{
    public int Id{get;}

    /// <summary>
    /// FK of a mediaType.
    /// </summary>
    public Guid MediaId {get;}

    /// <summary>
    /// Name of the character played in the media.
    /// </summary>
    public string CharacterName { get; }

    /// <summary>
    /// Used to order cast in credits.
    /// </summary>
    public int CreditIndex{get;}

    /// <summary>
    /// The actor that plays the character in the media.
    /// </summary>
    public Actor Actor{get;}

    public CastMember(string characterName, int creditIndex, Actor actor)
    {
        if (string.IsNullOrEmpty(characterName)) throw new ArgumentNullException(nameof(characterName));
        CharacterName = characterName.Trim();

        CreditIndex = creditIndex;

        ArgumentNullException.ThrowIfNull(actor);
        Actor = actor;
    }

    // EF materialization.
    private CastMember(int id, Guid mediaId, string characterName, int creditIndex)
    {
        Id = id;
        MediaId = mediaId;
        CharacterName = characterName;
        CreditIndex = creditIndex;
        Actor = null!; // suppress warning.
    }

}