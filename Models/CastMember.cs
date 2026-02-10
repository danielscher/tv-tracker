namespace TvTracker.Models;

/// <summary>
/// Represents a role or a character played by an actor in some media.
/// </summary>
/// <param name="name">name of the role / character in the media</param>
/// <param name="creditIndex"> place in the list of credits from 0 to N with 0 being the first </param>
public class CastMember(in string name, in int creditIndex, in Actor actor)
{
    public int Id{get;private set;}

    public string CharacterName { get; private set; } = name;

    public int CreditIndex{get;private set;} = creditIndex;

    public Actor Actor{get;set;} = actor;
}