namespace Common.Contracts;

/// <summary>
/// Sun position parameters
/// </summary>
public sealed class Sun
{
    public Sun(DateTime dateTime,
        double gst = 0,
        double slong = 0,
        double srasn = 0,
        double sdec = 0)
    {
        DateTime = dateTime;
        Gst = gst;
        Slong = slong;
        Srasn = srasn;
        Sdec = sdec;
    }

    /// <summary>
    /// Input UTC date and time
    /// </summary>
    public DateTime DateTime { get; }

    /// <summary>
    /// Greenwich mean sidereal time
    /// </summary>
    public double Gst { get; }

    /// <summary>
    /// Longitude along ecliptic
    /// </summary>
    public double Slong { get; }

    /// <summary>
    /// Right ascension
    /// </summary>
    public double Srasn { get; }

    /// <summary>
    /// Declination of the Sun (radians)
    /// </summary>
    public double Sdec { get; }
}
