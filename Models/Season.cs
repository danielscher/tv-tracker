namespace TvTracker.Models;

public class Season(in int seasonsNumber, in int episodes, in int episodeLength)
{
    public int SeasonNumber{get;private set;} = seasonsNumber;

    /// <summary>
    /// Number of episodes in this season.
    /// </summary>
    public int Episodes{get;private set;} = episodes;

    /// <summary>
    /// Length of each episode in minutes.
    /// Assumes the same length for each episode as typically this doesn't very much. 
    /// </summary>
    public int EpisodeLength{get;private set;} = episodeLength;
}