namespace TvTracker.Models;
public class Movie : Media
{

    /// <summary>
    /// Movie length in minutes.
    /// </summary>
    public int Length{get;}

    public int ReleaseYear{get;}

    public Movie(MediaMetaInfo mediaInfo, int length, int releaseYear) : base(mediaInfo)
    {
        Length = length;
        ReleaseYear = releaseYear;
    }

    // EF materialization.
    private Movie(int length, int releaseYear) : base()
    {
        Length = length;
        ReleaseYear = releaseYear;
    }

}
