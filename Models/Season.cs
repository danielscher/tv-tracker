namespace TvTracker.Models;

public class Season(int seasonNumber, int episodes, int episodeLength)
{
    public Guid Id {get;}

    public int TmdbId {get;}

    /// <summary>
    /// FK for EF.
    /// </summary>
    public Guid SeriesId {get;}

    /// <summary>
    /// I.e., (Season) 1, 2, 3, ...etc.
    /// </summary>
    public int SeasonNumber{get;} = seasonNumber;

    /// <summary>
    /// Number of episodes in this season.
    /// </summary>
    public int Episodes{get;} = episodes;

    /// <summary>
    /// Length of each episode in minutes.
    /// Assumes the same length for each episode as typically this doesn't very much. 
    /// </summary>
    public int EpisodeLength{get;} = episodeLength;
}