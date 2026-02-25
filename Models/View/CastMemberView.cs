namespace TvTracker.Models.View;
public class CastMemberView
{
    public string CharacterName {get;} 
    public int ActorId {get;}
    public string ActorName {get;}
    public string? PosterPath {get;}

    public CastMemberView(CastMemberResponse memberResponse)
    {
        CharacterName = memberResponse.CharacterName;
        ActorId = memberResponse.ActorId;
        ActorName = memberResponse.ActorName;
        PosterPath = memberResponse.PosterPath;
    }
}
